using Starfish.Core.Models;

namespace Starfish.Core.Services;

public interface IBankTransactionsService
{
    public Task<List<BankTransaction>> GetAllAsync(CancellationToken ctx);
    
    public Task<BankTransaction?> GetAsync(Guid id, CancellationToken ctx);
    
    public Task AddAsync(BankTransaction transaction, CancellationToken ctx);
    
    public Task DeleteAsync(Guid id, CancellationToken ctx);
}