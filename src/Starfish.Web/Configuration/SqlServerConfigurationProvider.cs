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
            _timer = new Timer(ReloadSettings, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(Source.PeriodInSeconds));
        }
    }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        Source.OptionsAction(builder);
        using (var dbContext = new DataContext(builder.Options))
        {
            try
            {
                dbContext.Database.EnsureCreated();

                Data = dbContext.StarfishSettings.ToDictionary<StarfishSettings, string, string?>(c => c.Id, c => c.Value, StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                // Ignore
            }
        }
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
