using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.Data;
using Starfish.Shared;
using Starfish.Web.Options;

namespace Starfish.Web.Configuration;

public class StarfishConfigurationProvider : ConfigurationProvider, IDisposable
{
    public StarfishConfigurationSource Source { get; }

    private readonly Timer? _timer;
    
    public StarfishConfigurationProvider(StarfishConfigurationSource source)
    {
        Source = source;

        if (Source.ReloadPeriodically)
        {
            _timer = new Timer(ReloadSettings, null, TimeSpan.Zero, TimeSpan.FromSeconds(Source.PeriodInSeconds));
        }
    }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        Source.OptionsAction(builder);
        
        using var dbContext = new DataContext(builder.Options);
        
        dbContext.Database.EnsureCreated();

        Data = dbContext.StarfishSettings.Any()
            ? dbContext.StarfishSettings.ToDictionary<StarfishSettings, string, string?>(c => c.Id, c => c.Value, StringComparer.OrdinalIgnoreCase)
            : CreateAndSaveDefaultValues(dbContext);
    }

    private static IDictionary<string, string?> CreateAndSaveDefaultValues(
        DataContext context)
    {
        var settings = new Dictionary<string, string?>(
            StringComparer.OrdinalIgnoreCase)
        {
            [$"{nameof(StarfishLoggingOptions)}:{nameof(StarfishLoggingOptions.RequestLoggingEnabled)}"] = "false"
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
