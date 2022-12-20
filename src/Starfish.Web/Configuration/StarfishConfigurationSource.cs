using Microsoft.EntityFrameworkCore;

namespace Starfish.Web.Configuration;

public class StarfishConfigurationSource : IConfigurationSource
{
    public required Action<DbContextOptionsBuilder> OptionsAction { get; init; }
    
    public bool ReloadPeriodically { get; init; }

    public int PeriodInSeconds { get; init; } = 5;

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new StarfishConfigurationProvider(this);
}