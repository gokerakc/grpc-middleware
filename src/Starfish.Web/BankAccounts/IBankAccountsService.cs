using Starfish.Core.Models;

namespace Starfish.Web.BankAccounts;

public interface IBankAccountsService
{
    public Task<List<BankAccount>> GetAll(CancellationToken ctx);
    
    public Task<BankAccount?> Get(Guid id, CancellationToken ctx);
    
    public Task Add(BankAccount account, CancellationToken ctx);
    
    public Task Add(List<BankAccount> accounts, CancellationToken ctx);
    
    public Task Delete(Guid id, CancellationToken ctx);
    
    public Task Delete(List<Guid> ids, CancellationToken ctx);
}