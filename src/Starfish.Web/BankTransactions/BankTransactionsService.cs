﻿using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;

namespace Starfish.Web.BankTransactions;

public class BankTransactionsService : IBankTransactionsService
{
    private readonly DataContext _context;

    public BankTransactionsService(DataContext context)
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

    public async Task Add(List<BankTransaction> accounts, CancellationToken ctx)
    {
        await _context.BankTransactions.AddRangeAsync(accounts, ctx);
        await _context.SaveChangesAsync(ctx);
    }

    public async Task Delete(Guid id, CancellationToken ctx)
    {
        await _context.BankTransactions
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ctx);
    }

    public async Task Delete(List<Guid> ids, CancellationToken ctx)
    {
        await _context.BankTransactions
            .Where(x => ids.Contains(x.Id))
            .ExecuteDeleteAsync(ctx);
    }
}