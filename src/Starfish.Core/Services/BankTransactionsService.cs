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
    
    public async Task<List<BankTransaction>> GetAll(CancellationToken ctx) => await _bankTransactionsRepository.GetAll(ctx);

    public async Task<BankTransaction?> Get(Guid id, CancellationToken ctx) => await _bankTransactionsRepository.Get(id, ctx);

    public async Task Add(BankTransaction transaction, CancellationToken ctx) => await _bankTransactionsRepository.Add(transaction, ctx);

    public async Task Add(IEnumerable<BankTransaction> transactions, CancellationToken ctx) => await _bankTransactionsRepository.Add(transactions, ctx);
    
    public async Task Delete(Guid id, CancellationToken ctx) => await _bankTransactionsRepository.Delete(id, ctx);

    public async Task Delete(List<Guid> ids, CancellationToken ctx) => await _bankTransactionsRepository.Delete(ids, ctx);
}