using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Shared;

namespace Starfish.Infrastructure.Repositories;

public class BankAccountsRepository : IRepository<BankAccount>
{
    private readonly DataContext _context;
    private readonly DbSet<BankAccount>  _bankAccounts;

    public BankAccountsRepository(DataContext context)
    {
        _context = context;
        _bankAccounts = context.Set<BankAccount>();
    }
    
    public async Task<List<BankAccount>> GetAllAsync(CancellationToken ctx)
    {
        return await _bankAccounts
            .AsNoTracking()
            .ToListAsync(ctx);
    }

    public BankAccount? Get(Guid id)
    {
        return _bankAccounts
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
    }

    public async Task<BankAccount?> GetAsync(Guid id, CancellationToken ctx)
    {
        return await _bankAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ctx);
    }

    public async Task AddAsync(BankAccount account, CancellationToken ctx)
    {
        await _bankAccounts.AddAsync(account, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public void Add(BankAccount account)
    {
        _bankAccounts.Add(account);
        _context.SaveChangesAsync();
    }

    public async Task AddAsync(IEnumerable<BankAccount> accounts, CancellationToken ctx)
    {
        await _bankAccounts.AddRangeAsync(accounts, ctx);
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