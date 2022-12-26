using Starfish.Core.Models;

namespace Starfish.Core.Services;

public interface IBankAccountsService 
{
    public Task<List<BankAccount>> GetAllAsync(CancellationToken ctx);
    
    public Task<BankAccount?> GetAsync(Guid id, CancellationToken ctx);
    
    public Task AddAsync(BankAccount account, CancellationToken ctx);
    
    public Task DeleteAsync(Guid id, CancellationToken ctx);
}