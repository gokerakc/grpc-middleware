using Microsoft.EntityFrameworkCore;

namespace Starfish.Web.Configuration;

public class StarfishConfigurationSource : IConfigurationSource
{
    public required Action<DbContextOptionsBuilder> OptionsAction { get; init; }
    
    public bool ReloadOnChange { get; init; }

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new StarfishConfigurationProvider(this);
}