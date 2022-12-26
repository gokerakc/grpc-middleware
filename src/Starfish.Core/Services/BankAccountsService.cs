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
    
    public async Task<List<BankAccount>> GetAllAsync(CancellationToken ctx) => await _bankAccountRepository.GetAllAsync(ctx);

    public async Task<BankAccount?> GetAsync(Guid id, CancellationToken ctx) => await _bankAccountRepository.GetAsync(id, ctx);

    public async Task AddAsync(BankAccount account, CancellationToken ctx) => await _bankAccountRepository.AddAsync(account, ctx);
    
    public async Task DeleteAsync(Guid id, CancellationToken ctx) => await _bankAccountRepository.DeleteAsync(id, ctx);
}