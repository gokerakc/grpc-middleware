using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Shared;

namespace Starfish.Infrastructure.Repositories;

public class BankAccountsRepository : IRepository<BankAccount>
{
    private readonly DataContext _context;

    public BankAccountsRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<BankAccount>> GetAll(CancellationToken ctx)
    {
        return await _context.BankAccounts
            .AsNoTracking()
            .ToListAsync(ctx);
    }

    public async Task<BankAccount?> Get(Guid id, CancellationToken ctx)
    {
        return await _context.BankAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ctx);
    }

    public async Task Add(BankAccount account, CancellationToken ctx)
    {
        await _context.BankAccounts.AddAsync(account, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Add(IEnumerable<BankAccount> accounts, CancellationToken ctx)
    {
        await _context.BankAccounts.AddRangeAsync(accounts, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Delete(Guid id, CancellationToken ctx)
    {
        var entity = await _context.BankAccounts
            .Where(x => x.Id == id)
            .FirstAsync(ctx);
        
        _context.Remove(entity);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Delete(List<Guid> ids, CancellationToken ctx)
    {
        var entities = await _context.BankAccounts
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ctx);
        
        _context.RemoveRange(entities);

        await _context.SaveChangesAsync(ctx);
    }
}