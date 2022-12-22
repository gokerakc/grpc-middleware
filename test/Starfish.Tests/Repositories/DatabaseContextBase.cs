using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.Data;

namespace Starfish.Tests.Repositories;

public static class DatabaseContextBase
{
    public static DataContext GetDataContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new DataContext(optionsBuilder.Options);
    }
}