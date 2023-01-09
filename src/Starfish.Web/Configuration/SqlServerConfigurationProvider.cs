using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.Data;
using Starfish.Shared;

namespace Starfish.Web.Configuration;

public class SqlServerConfigurationProvider : ConfigurationProvider, IDisposable
{
    private SqlServerConfigurationSource Source { get; }
    private readonly Timer? _timer;

    public SqlServerConfigurationProvider(SqlServerConfigurationSource source)
    {
        Source = source;

        if (Source.ReloadPeriodically)
        {
            _timer = new Timer
            (
                callback: ReloadSettings,
                dueTime: TimeSpan.FromSeconds(10),
                period: TimeSpan.FromSeconds(Source.PeriodInSeconds),
                state: null
            );
        }
    }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        Source.OptionsAction(builder);
        
        using (var dbContext = new DataContext(builder.Options))
        {
            dbContext.Database.EnsureCreated();
            
            Data = dbContext.StarfishSettings.Any()
                ? dbContext.StarfishSettings
                    .ToDictionary<StarfishSettings, string, string?>(c => c.Id, c => c.Value,
                    StringComparer.OrdinalIgnoreCase)
                : CreateAndSaveDefaultValues(dbContext);
        }
    }

    private static IDictionary<string, string?> CreateAndSaveDefaultValues(DataContext context)
    {
        var settings = new Dictionary<string, string?>(
            StringComparer.OrdinalIgnoreCase)
        {
            [$"{nameof(StarfishOptions)}:{nameof(StarfishOptions.FraudCheckerEnabled)}"] = "false",
            [$"{nameof(StarfishOptions)}:{nameof(StarfishOptions.PerformanceMonitorEnabled)}"] = "false"
        };

        context.StarfishSettings.AddRange(
            settings.Select(kvp => new StarfishSettings(kvp.Key, kvp.Value!))
                .ToArray());

        context.SaveChanges();

        return settings;
    }

    private void ReloadSettings(object? state)
    {
        Load();
        OnReload();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
