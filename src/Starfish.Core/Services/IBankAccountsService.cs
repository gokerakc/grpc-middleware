using Starfish.Core.Models;

namespace Starfish.Core.Services;

public interface IBankAccountsService 
{
    public Task<List<BankAccount>> GetAll(CancellationToken ctx);
    
    public Task<BankAccount?> Get(Guid id, CancellationToken ctx);
    
    public Task Add(BankAccount account, CancellationToken ctx);
    
    public Task Delete(Guid id, CancellationToken ctx);
}