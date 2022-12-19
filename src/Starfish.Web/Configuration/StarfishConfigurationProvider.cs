using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.Data;
using Starfish.Shared;
using Starfish.Web.Options;

namespace Starfish.Web.Configuration;

public class StarfishConfigurationProvider : ConfigurationProvider
{
    private readonly StarfishConfigurationSource _source;
    private readonly Timer? _timer;
    
    public StarfishConfigurationProvider(StarfishConfigurationSource source)
    {
        _source = source;

        if (_source.ReloadOnChange)
        {
            _timer = new Timer(ReloadSettings, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
    }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        _source.OptionsAction(builder);
        
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
            settings.Select(kvp => new StarfishSettings(kvp.Key, kvp.Value))
                .ToArray());

        context.SaveChanges();

        return settings;
    }
    
    private void ReloadSettings(object? state)
    {
        Load();
        OnReload();
    }
}
