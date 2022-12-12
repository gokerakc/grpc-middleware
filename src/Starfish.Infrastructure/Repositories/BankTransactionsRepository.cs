using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Shared;

namespace Starfish.Infrastructure.Repositories;

public class BankTransactionsRepository : IRepository<BankTransaction>
{
    private readonly DataContext _context;

    public BankTransactionsRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<BankTransaction>> GetAll(CancellationToken ctx)
    {
        return await _context.BankTransactions
            .AsNoTracking()
            .ToListAsync(ctx);
    }

    public async Task<BankTransaction?> Get(Guid id, CancellationToken ctx)
    {
        return await _context.BankTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ctx);
    }

    public async Task Add(BankTransaction account, CancellationToken ctx)
    {
        await _context.BankTransactions.AddAsync(account, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Add(IEnumerable<BankTransaction> accounts, CancellationToken ctx)
    {
        await _context.BankTransactions.AddRangeAsync(accounts, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Delete(Guid id, CancellationToken ctx)
    {
        var entity = await _context.BankTransactions
            .Where(x => x.Id == id)
            .FirstAsync(ctx);
        
        _context.Remove(entity);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Delete(List<Guid> ids, CancellationToken ctx)
    {
        var entities = await _context.BankTransactions
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ctx);
        
        _context.RemoveRange(entities);

        await _context.SaveChangesAsync(ctx);
    }
}