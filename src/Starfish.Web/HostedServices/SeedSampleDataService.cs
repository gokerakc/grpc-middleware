using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.DTOs;

namespace Starfish.Web.HostedServices;

public class SeedSampleDataService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeedSampleDataService> _logger;
    private List<BankAccountDto>? _bankAccounts;

    public SeedSampleDataService(IServiceProvider serviceProvider, ILogger<SeedSampleDataService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            if (dbContext.BankAccounts.Any() && dbContext.BankTransactions.Any())
            {
                return;
            }

            _logger.LogInformation("Sample data seeding process has been started.");

            await SeedBankAccounts(dbContext, cancellationToken);

            await SeedBankTransactions(dbContext, cancellationToken);
        }
        _logger.LogInformation("Sample data seeding process has been finished.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    private async Task SeedBankAccounts(DataContext dbContext, CancellationToken cancellationToken)
    {
        _bankAccounts = new List<BankAccountDto>();

        for (var i = 0; i < 1000; i++)
        {
            _bankAccounts.Add(new BankAccountDto
            {
                Id = Guid.NewGuid(),
                AccountName = $"{_firstnames[Random.Shared.Next(_firstnames.Count)]} {_lastnames[Random.Shared.Next(_lastnames.Count)]}",
                AccountNumber = Random.Shared.Next(10000000, 99999999).ToString(),
                Balance = Random.Shared.Next(0, 1_000) * 10
            });
        }

        await dbContext.BankAccounts.AddRangeAsync(_bankAccounts, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Bank accounts has been added.");

    }
    
    private async Task SeedBankTransactions(DataContext dbContext, CancellationToken cancellationToken)
    {
        var bankTransactions = new List<BankTransactionDto>();

        var bankAccountIds = _bankAccounts!.Select(x => x.Id).ToList();

        for (var i = 0; i < 50_000; i++)
        {
            bankTransactions.Add(new BankTransactionDto
            {
                SourceId = bankAccountIds[Random.Shared.Next(0, 500)],
                TargetId = bankAccountIds[Random.Shared.Next(500, 999)],
                Amount = Random.Shared.Next(0, 1_000) * 10,
                Date = new DateTime(2022, Random.Shared.Next(1,12), Random.Shared.Next(1,28))
            });
        }

        await dbContext.BankTransactions.AddRangeAsync(bankTransactions, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Bank transactions has been added.");
    }
    

    private readonly List<string> _firstnames = new()
    {
        "Skylar","Luciano","Kobe","Jeramiah","Linda","Gauge","Kamron","Lizeth","Janiya","Kennedi","Kai","German",
        "Ashleigh","Luke","Arturo","Nash","Phoenix","Gilberto","Alden","Kaitlyn","Mina","Ellie","Shelby","Madden",
        "Alma","Clare","Marco","Serena"
    };

    private readonly List<string> _lastnames = new()
    {
        "Patterson","Suarez","Goodman","Gillespie","Novak","Mcintosh","Medina","Benton","Romero","Brady","Hines","Hopkins",
        "Newton","Hood","Andrews","Guerrero","Barry","Spence","Villanueva","Ingram","Stevenson","Burnett","Griffith",
        "Washington","Williams","Ryan","Ochoa","Kerr"
    };
}