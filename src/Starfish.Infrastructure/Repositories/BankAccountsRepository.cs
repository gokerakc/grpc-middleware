using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.DTOs;
using Starfish.Shared;

namespace Starfish.Infrastructure.Repositories;

public class BankAccountsRepository : IRepository<BankAccount>
{
    private readonly DataContext _context;
    private readonly DbSet<BankAccountDto>  _bankAccounts;

    public BankAccountsRepository(DataContext context)
    {
        _context = context;
        _bankAccounts = context.Set<BankAccountDto>();
    }
    
    public async Task<List<BankAccount>> GetAllAsync(CancellationToken ctx)
    {
        return await _bankAccounts
            .AsNoTracking()
            .Select(x => (BankAccount)x)
            .ToListAsync(ctx);
    }

    public BankAccount? Get(Guid id)
    {
        return _bankAccounts
            .AsNoTracking()
            .Select(x => (BankAccount)x)
            .FirstOrDefault(x => x.Id == id);
    }

    public async Task<BankAccount?> GetAsync(Guid id, CancellationToken ctx)
    {
        var result = await _bankAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ctx);

        return result is null ? null : (BankAccount)result;
    }

    public async Task AddAsync(BankAccount account, CancellationToken ctx)
    {
        await _bankAccounts.AddAsync(BankAccountDto.FromBankAccount(account), ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public void Add(BankAccount account)
    {
        _bankAccounts.Add(BankAccountDto.FromBankAccount(account));
        _context.SaveChangesAsync();
    }

    public async Task AddAsync(IEnumerable<BankAccount> accounts, CancellationToken ctx)
    {
        var bankAccountDtos = accounts.Select(BankAccountDto.FromBankAccount).ToList();
        await _bankAccounts.AddRangeAsync(bankAccountDtos, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ctx)
    {
        var entity = await _bankAccounts
            .Where(x => x.Id == id)
            .FirstAsync(ctx);
        
        _context.Remove(entity);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task DeleteAsync(List<Guid> ids, CancellationToken ctx)
    {
        var entities = await _bankAccounts
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ctx);
        
        _context.RemoveRange(entities);

        await _context.SaveChangesAsync(ctx);
    }
}