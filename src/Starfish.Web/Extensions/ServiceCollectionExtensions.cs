using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Core.Services;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.Repositories;
using Starfish.Shared;
using Starfish.Web.Configuration;
using Starfish.Web.Exceptions;
using Starfish.Web.Middlewares;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using Starfish.Core;
using Starfish.Infrastructure;
using Starfish.Infrastructure.Services;
using Starfish.Web.HostedServices;

namespace Starfish.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilog(this IServiceCollection serviceCollection, ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((builderContext, _, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(builderContext.Configuration);
        });

        return serviceCollection;
    }
    public static IServiceCollection AddApiVersioningDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
            //opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });

        serviceCollection.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'V'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
        return serviceCollection;
    }

    public static IServiceCollection AddStarfishDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IRepository<BankAccount>, BankAccountsRepository>()
            .AddScoped<IRepository<BankTransaction>, BankTransactionsRepository>()
            .AddScoped<IBankAccountsService, BankAccountsService>()
            .AddScoped<IBankTransactionsService, BankTransactionsService>()
            .AddScoped<IFraudCheckerService, FraudCheckerService>();
        
        serviceCollection.AddSingleton<PerformanceMonitorMiddleware>();


        return serviceCollection;
    }

    public static IServiceCollection AddFraudCheckerGrpcClient(this IServiceCollection serviceCollection, ConfigurationManager configuration)
    {
        var grpcServiceAddress = configuration.GetConnectionString("GrpcServiceConnection");
        
        if (grpcServiceAddress is null || string.Equals(grpcServiceAddress, "<PLACEHOLDER>", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("Grpc service address is missing.");

        serviceCollection.AddGrpcClient<FraudChecker.FraudCheckerClient>(o =>
        {
            o.Address = new Uri(grpcServiceAddress);
        });

        return serviceCollection;
    }

    public static IServiceCollection AddStarfishDatabase(this IServiceCollection serviceCollection,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(connectionString,
                providerOptions =>
                    providerOptions
                        .CommandTimeout(60)
                        .EnableRetryOnFailure());
        });

        return serviceCollection;
    }
    
    public static IServiceCollection AddStarfishConfigurationSource(this IServiceCollection serviceCollection,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        configuration.Sources.Add(new SqlServerConfigurationSource
        {
            OptionsAction = (optionsBuilder) => optionsBuilder.UseSqlServer(connectionString),
            ReloadPeriodically = true,
            PeriodInSeconds = 5
        });
        
        serviceCollection.Configure<StarfishOptions>(configuration.GetSection(nameof(StarfishOptions)));

        return serviceCollection;
    }
    
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection serviceCollection, bool isDevelopment)
    {
        serviceCollection.AddProblemDetails((o) =>
            {
                ProblemDetailsHelper.ConfigureProblemDetails(o, isDevelopment);
            })
            .AddControllers()
            // Adds MVC conventions to work better with the ProblemDetails middleware.
            .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        return serviceCollection;
    }
    
    public static IServiceCollection AddStarfishHostedServices(this IServiceCollection serviceCollection, bool isDevelopment)
    {
        serviceCollection.AddHostedService<DatabaseMigrationService>();

        if (isDevelopment)
        {
            serviceCollection.AddHostedService<SeedSampleDataService>();    
        }

        return serviceCollection;
    }
}