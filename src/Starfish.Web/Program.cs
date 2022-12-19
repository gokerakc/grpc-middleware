using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Starfish.Core.Models;
using Starfish.Core.Services;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.Repositories;
using Starfish.Shared;
using Starfish.Web;
using Starfish.Web.Configuration;
using Starfish.Web.HostedServices;
using Starfish.Web.Middlewares;
using Starfish.Web.Options;

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog((builderContext, serviceProvider, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(builderContext.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    ReloadOnChange = true
});

// Starfish options configuration
builder.Services.Configure<StarfishLoggingOptions>(builder.Configuration.GetSection("StarfishLoggingOptions"));

// Starfish middleware injections
builder.Services.AddSingleton<RequestLoggerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();