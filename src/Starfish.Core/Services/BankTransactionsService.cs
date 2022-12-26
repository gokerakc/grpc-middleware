using Starfish.Core.Models;
using Starfish.Shared;

namespace Starfish.Core.Services;

public class BankTransactionsService : IBankTransactionsService
{
    private readonly IRepository<BankTransaction> _bankTransactionsRepository;

    public BankTransactionsService(IRepository<BankTransaction> bankTransactionRepository)
    {
        _bankTransactionsRepository = bankTransactionRepository;
    }
    
    public async Task<List<BankTransaction>> GetAllAsync(CancellationToken ctx) => await _bankTransactionsRepository.GetAllAsync(ctx);

    public async Task<BankTransaction?> GetAsync(Guid id, CancellationToken ctx) => await _bankTransactionsRepository.GetAsync(id, ctx);

    public async Task AddAsync(BankTransaction transaction, CancellationToken ctx) => await _bankTransactionsRepository.AddAsync(transaction, ctx);
    
    public async Task DeleteAsync(Guid id, CancellationToken ctx) => await _bankTransactionsRepository.DeleteAsync(id, ctx);
}