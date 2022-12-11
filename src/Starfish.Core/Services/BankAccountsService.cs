using Starfish.Core.Models;
using Starfish.Shared;

namespace Starfish.Core.Services;

public class BankAccountsService : IBankAccountsService
{
    private readonly IRepository<BankAccount> _bankAccountRepository;

    public BankAccountsService(IRepository<BankAccount> bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }
    
    public async Task<List<BankAccount>> GetAll(CancellationToken ctx) => await _bankAccountRepository.GetAll(ctx);

    public async Task<BankAccount?> Get(Guid id, CancellationToken ctx) => await _bankAccountRepository.Get(id, ctx);

    public async Task Add(BankAccount account, CancellationToken ctx) => await _bankAccountRepository.Add(account, ctx);
    
    public async Task Delete(Guid id, CancellationToken ctx) => await _bankAccountRepository.Delete(id, ctx);
}