using Starfish.Infrastructure.Data;
using Starfish.Shared;

namespace Starfish.Web.HostedServices;

public class PopulateDefaultSettingsService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PopulateDefaultSettingsService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            if (dbContext.StarfishSettings.Any())
            {
                return;
            }
            
            var settings = new Dictionary<string, string?>(
                StringComparer.OrdinalIgnoreCase)
            {
                [$"{nameof(StarfishOptions)}:{nameof(StarfishOptions.PerformanceMonitorEnabled)}"] = "false",
                [$"{nameof(StarfishOptions)}:{nameof(StarfishOptions.FraudCheckerEnabled)}"] = "false"
            };
        
            dbContext.StarfishSettings.AddRange(
                settings.Select(kvp => new StarfishSettings(kvp.Key, kvp.Value!))
                    .ToArray());
        
            await dbContext.SaveChangesAsync(cancellationToken);

        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}