using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Shared;

namespace Starfish.Infrastructure.Repositories;

public class BankTransactionsRepository : IRepository<BankTransaction>
{
    private readonly DataContext _context;
    private readonly DbSet<BankTransaction> _bankTransactions;

    public BankTransactionsRepository(DataContext context)
    {
        _context = context;
        _bankTransactions = context.Set<BankTransaction>();
    }
    
    public async Task<List<BankTransaction>> GetAllAsync(CancellationToken ctx)
    {
        return await _bankTransactions
            .AsNoTracking()
            .ToListAsync(ctx);
    }
    
    public async Task<BankTransaction?> GetAsync(Guid id, CancellationToken ctx)
    {
        return await _bankTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ctx);
    }

    public async Task AddAsync(BankTransaction account, CancellationToken ctx)
    {
        await _bankTransactions.AddAsync(account, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task AddAsync(IEnumerable<BankTransaction> accounts, CancellationToken ctx)
    {
        await _bankTransactions.AddRangeAsync(accounts, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ctx)
    {
        var entity = await _bankTransactions
            .Where(x => x.Id == id)
            .FirstAsync(ctx);
        
        _context.Remove(entity);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task DeleteAsync(List<Guid> ids, CancellationToken ctx)
    {
        var entities = await _bankTransactions
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ctx);
        
        _context.RemoveRange(entities);

        await _context.SaveChangesAsync(ctx);
    }
}