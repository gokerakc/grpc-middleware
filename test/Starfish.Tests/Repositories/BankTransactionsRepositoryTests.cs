using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.Repositories;

namespace Starfish.Tests.Repositories;

public class BankTransactionsRepositoryTests
{
    private readonly BankTransactionsRepository _subject;
    private readonly DataContext _context;
    private readonly CancellationToken _ctx = CancellationToken.None;

    private readonly Guid _sourceId = Guid.NewGuid();
    private readonly Guid _targetId = Guid.NewGuid();

    public BankTransactionsRepositoryTests()
    {
        _context = DatabaseContextBase.GetDataContext();
        _subject = new BankTransactionsRepository(_context);
    }

    [Fact]
    public async Task Add_ShouldAddItemAsExpected()
    {
        var id = Guid.NewGuid();
        
        var bankTransaction = new BankTransaction { Id= id, TargetId = _targetId, SourceId = _sourceId, Amount = 1500, Description = "Test desc"};

        await _subject.AddAsync(bankTransaction, _ctx);

        var result = await _context.BankTransactions.FirstOrDefaultAsync(x => x.Id == id, _ctx);
        
        Assert.NotNull(result);
        Assert.Equal(result, bankTransaction);
    }

    [Fact]
    public async Task Add_ShouldAddItemsAsExpected()
    {
        var id01 = Guid.NewGuid();
        var id02 = Guid.NewGuid();

        var bankTransactions = new List<BankTransaction>
        {
            new BankTransaction { Id = id01, TargetId = _targetId, SourceId = _sourceId, Amount = 100, Description = "Test desc 01" },
            new BankTransaction { Id = id02, TargetId = _targetId, SourceId = _sourceId, Amount = 1500, Description = "Test desc 02" }
        };

        await _subject.AddAsync(bankTransactions, _ctx);

        var result = await _context.BankTransactions
            .Select(x => x)
            .Where(x => x.Id == id01 || x.Id == id02)
            .ToListAsync(_ctx);

        Assert.NotEmpty(result);
        Assert.Equal(result, bankTransactions);
    }

    [Fact]
    public async Task Get_ShouldGetItemAsExpected()
    {
        var id = Guid.NewGuid();
        var bankTransaction = new BankTransaction { Id= id, TargetId = _targetId, SourceId = _sourceId, Amount = 500, Description = "Test desc 02"};

        await _context.BankTransactions.AddAsync(bankTransaction, _ctx);
        await _context.SaveChangesAsync(_ctx);

        var result = await _subject.GetAsync(id, _ctx);
        
        Assert.NotNull(result);
        Assert.Equal(result, bankTransaction);
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteItemAsExpected()
    {
        var id = Guid.NewGuid();
        var bankTransaction = new BankTransaction { Id= id, TargetId = _targetId, SourceId = _sourceId, Amount = 100, Description = "Test desc 03"};

        await _context.BankTransactions.AddAsync(bankTransaction, _ctx);
        await _context.SaveChangesAsync(_ctx);

        await _subject.DeleteAsync(id, _ctx);
        
        var result = await _context.BankTransactions.FirstOrDefaultAsync(x => x.Id == id, _ctx);
        
        Assert.Null(result);
    }
}