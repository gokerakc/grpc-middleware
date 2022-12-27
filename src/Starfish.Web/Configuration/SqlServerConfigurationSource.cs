using Microsoft.EntityFrameworkCore;

namespace Starfish.Web.Configuration;

public class SqlServerConfigurationSource : IConfigurationSource
{
    public required Action<DbContextOptionsBuilder> OptionsAction { get; init; }
    
    public required ILoggerFactory LoggerFactory { get; init; }
    
    public bool ReloadPeriodically { get; init; }

    public int PeriodInSeconds { get; init; } = 5;

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new SqlServerConfigurationProvider(this);
}