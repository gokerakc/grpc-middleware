using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.Data;

namespace Starfish.Tests.Repositories;

public static class DatabaseContextBase
{
    private static DataContext? _dataContext;
    
    public static DataContext GetDataContext()
    {
        if (_dataContext is not null)
        {
            return _dataContext;
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("starfish-db-test");

        _dataContext = new DataContext(optionsBuilder.Options);
        return _dataContext;
    }
}