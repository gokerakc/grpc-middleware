using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.DTOs;
using Starfish.Shared;

namespace Starfish.Infrastructure.Repositories;

public class BankTransactionsRepository : IRepository<BankTransaction>
{
    private readonly DataContext _context;
    private readonly DbSet<BankTransactionDto> _bankTransactions;

    public BankTransactionsRepository(DataContext context)
    {
        _context = context;
        _bankTransactions = context.Set<BankTransactionDto>();
    }
    
    public async Task<List<BankTransaction>> GetAllAsync(CancellationToken ctx)
    {
        return await _bankTransactions
            .AsNoTracking()
            .Select(x => (BankTransaction)x)
            .ToListAsync(ctx);
    }
    
    public async Task<BankTransaction?> GetAsync(Guid id, CancellationToken ctx)
    {
        var result = await _bankTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ctx);

        return result is null ? null : (BankTransaction)result;
    }

    public async Task AddAsync(BankTransaction transaction, CancellationToken ctx)
    {
        await _bankTransactions.AddAsync(BankTransactionDto.FromBankTransaction(transaction), ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task AddAsync(IEnumerable<BankTransaction> transactions, CancellationToken ctx)
    {
        var transactionDtos = transactions.Select(BankTransactionDto.FromBankTransaction).ToList();
        await _bankTransactions.AddRangeAsync(transactionDtos, ctx);
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