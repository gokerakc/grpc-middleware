using Microsoft.Extensions.Options;
using Starfish.Core.Models;
using Starfish.Shared;

namespace Starfish.Core.Services;

public class BankTransactionsService : IBankTransactionsService
{
    private readonly IRepository<BankTransaction> _bankTransactionsRepository;
    private readonly IOptionsMonitor<StarfishOptions> _options;
    private readonly IFraudCheckerService _fraudCheckerService;

    public BankTransactionsService(IRepository<BankTransaction> bankTransactionRepository, IFraudCheckerService fraudCheckerService, IOptionsMonitor<StarfishOptions> options)
    {
        _bankTransactionsRepository = bankTransactionRepository;
        _options = options;
        _fraudCheckerService = fraudCheckerService;
    }
    
    public async Task<List<BankTransaction>> GetAllAsync(CancellationToken ctx) => await _bankTransactionsRepository.GetAllAsync(ctx);

    public async Task<BankTransaction?> GetAsync(Guid id, CancellationToken ctx) => await _bankTransactionsRepository.GetAsync(id, ctx);

    public async Task AddAsync(BankTransaction transaction, CancellationToken ctx)
    {
        if (IsValidTransaction(transaction) == false)
        {
            // Throw a custom exception
            throw new Exception("Invalid transaction.");
        }
        
        await _bankTransactionsRepository.AddAsync(transaction, ctx);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ctx) => await _bankTransactionsRepository.DeleteAsync(id, ctx);
    
    private bool IsValidTransaction(BankTransaction transaction)
    {
        if (!_options.CurrentValue.FraudCheckerEnabled)
        {
            return true;
        }
        
        var result = _fraudCheckerService.Check(transaction);

        return result switch {
            FraudCheckResult.Suspicious or FraudCheckResult.Fraud => false,
            _ => true
        };

    }
}