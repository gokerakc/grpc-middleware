using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.Data;
using Starfish.Shared;
using Starfish.Web.Extensions;
using Starfish.Web.Options;

namespace Starfish.Web.Configuration;

public class SqlServerConfigurationProvider : ConfigurationProvider, IDisposable
{
    public SqlServerConfigurationSource Source { get; }

    private readonly Timer? _timer;

    private readonly ILogger<SqlServerConfigurationProvider> _logger;
    
    public SqlServerConfigurationProvider(SqlServerConfigurationSource source)
    {
        Source = source;

        _logger = source.LoggerFactory.CreateLogger<SqlServerConfigurationProvider>();

        if (Source.ReloadPeriodically)
        {
            _timer = new Timer(ReloadSettings, null, TimeSpan.Zero, TimeSpan.FromSeconds(Source.PeriodInSeconds));
        }
    }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        Source.OptionsAction(builder);

        using (var dbContext = new DataContext(builder.Options))
        {
            if (!dbContext.Database.CanConnect())
            {
                _logger.DatabaseConnectionError($"{nameof(SqlServerConfigurationProvider)} can not connect to the database.");
                return;
            }
            
            Data = dbContext.StarfishSettings.Any()
                ? dbContext.StarfishSettings.ToDictionary<StarfishSettings, string, string?>(c => c.Id, c => c.Value, StringComparer.OrdinalIgnoreCase)
                : CreateAndSaveDefaultValues(dbContext);
        }
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
