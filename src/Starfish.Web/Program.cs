using System.Text.Json.Serialization;
using Grpc.Net.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Starfish.Core.Models;
using Starfish.Core.Services;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.Repositories;
using Starfish.Shared;
using Starfish.Web;
using Starfish.Web.Configuration;
using Starfish.Web.Exceptions;
using Starfish.Web.HostedServices;
using Starfish.Web.Middlewares;
using Starfish.Web.Options;
using Starfish.Web.Watchers;

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog((builderContext, _, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(builderContext.Configuration);
});

// Add API versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
    //opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});

// Add ApiExplorer to discover versions
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'V'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


// Starfish injections
builder.Services.AddScoped<IRepository<BankAccount>, BankAccountsRepository>();
builder.Services.AddScoped<IRepository<BankTransaction>, BankTransactionsRepository>();
builder.Services.AddScoped<IBankAccountsService, BankAccountsService>();
builder.Services.AddScoped<IBankTransactionsService, BankTransactionsService>();

// Starfish hosted services
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedSampleDataService>();    
}

// Starfish GRPC client
var grpcServiceAddress = builder.Configuration.GetConnectionString("GrpcServiceConnection") ??
                            throw new ArgumentException("Grpc service address is missing.");
using var channel = GrpcChannel.ForAddress(grpcServiceAddress);
var requestLoggerClient = new RequestLogger.RequestLoggerClient(channel);
builder.Services.AddSingleton(requestLoggerClient);

// Starfish database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString,
        providerOptions =>
            providerOptions
                .CommandTimeout(60)
                .EnableRetryOnFailure());
});

// Add a new configuration source
builder.Configuration.Sources.Add(new StarfishConfigurationSource
{
    OptionsAction = (optionsBuilder) => optionsBuilder.UseSqlServer(connectionString),
    ReloadPeriodically = true,
    PeriodInSeconds = 5
});

// Starfish options configuration
builder.Services.Configure<StarfishLoggingOptions>(builder.Configuration.GetSection("StarfishLoggingOptions"));

// Starfish middleware injections
builder.Services.AddSingleton<RequestLoggerMiddleware>();

// Watchers (Just to try Change token feature)
WatcherHelper.AddGuestListWatcher();


// Add global exception handler (ProblemDetails)
builder.Services.AddProblemDetails((o) =>
    {
        ProblemDetailsHelper.ConfigureProblemDetails(o, builder.Environment.IsDevelopment());
    })
    .AddControllers()
    // Adds MVC conventions to work better with the ProblemDetails middleware.
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseProblemDetails();

app.UseMiddleware<RequestLoggerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();