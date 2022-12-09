using Starfish.Core.Models;

namespace Starfish.Web.BankTransactions;

public interface IBankTransactionsService
{
    public Task<List<BankTransaction>> GetAll(CancellationToken ctx);
    
    public Task<BankTransaction?> Get(Guid id, CancellationToken ctx);
    
    public Task Add(BankTransaction transaction, CancellationToken ctx);
    
    public Task Add(List<BankTransaction> transactions, CancellationToken ctx);
    
    public Task Delete(Guid id, CancellationToken ctx);
    
    public Task Delete(List<Guid> ids, CancellationToken ctx);
}