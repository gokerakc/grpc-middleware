using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Starfish.Web.Extensions;
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

builder.Services.AddApiVersioningDependencies()
    .AddStarfishDependencies()
    .AddStarfishGrpcClient(builder.Configuration)
    .AddStarfishDatabase(builder.Configuration)
    .AddStarfishConfigurationSource(builder.Configuration);



builder.Services.Configure<StarfishLoggingOptions>(builder.Configuration.GetSection("StarfishLoggingOptions"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Starfish hosted services
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedSampleDataService>();    
}

// Watchers (Just to try Change token feature)
WatcherHelper.AddGuestListWatcher();

builder.Services.AddGlobalExceptionHandler(builder.Environment.IsDevelopment());

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