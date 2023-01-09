using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.DTOs;
using Starfish.Infrastructure.Repositories;

namespace Starfish.Tests.Repositories;

public class BankAccountsRepositoryTests
{
    private readonly BankAccountsRepository _subject;
    private readonly DataContext _context;
    private readonly CancellationToken _ctx = CancellationToken.None;

    public BankAccountsRepositoryTests()
    {
        _context = DatabaseContextBase.GetDataContext();
        _subject = new BankAccountsRepository(_context);
    }

    [Fact]
    public async Task Add_ShouldAddItemAsExpected()
    {
        var id = Guid.NewGuid();
        
        var bankAccount = new BankAccount { Id= id, AccountName = "John Doe", AccountNumber = "88736728", Balance = 0 };

        await _subject.AddAsync(bankAccount, _ctx);

        var result = await _context.BankAccounts.FirstOrDefaultAsync(x => x.Id == id, _ctx);
        
        Assert.NotNull(result);
        Assert.Equal((BankAccount)result!, bankAccount);
    }
    
    [Fact]
    public async Task Add_ShouldAddItemsAsExpected()
    {
        var id01 = Guid.NewGuid();
        var id02 = Guid.NewGuid();
        
        var bankAccounts = new List<BankAccount>
        {
            new BankAccount { Id= id01, AccountName = "John Doe", AccountNumber = "88736728", Balance = 100 },
            new BankAccount { Id= id02, AccountName = "John Doe 02", AccountNumber = "11736728", Balance = 200 }
        };

        await _subject.AddAsync(bankAccounts, _ctx);

        var result = await _context.BankAccounts
            .Select(x => x)
            .Where(x => x.Id == id01 || x.Id == id02)
            .ToListAsync(_ctx);
        
        Assert.NotEmpty(result);
        Assert.True(bankAccounts.SequenceEqual(result.Select(x => (BankAccount)x)));
    }
    
    [Fact]
    public async Task Get_ShouldGetItemAsExpected()
    {
        var id = Guid.NewGuid();
        var bankAccount = new BankAccountDto { Id= id, AccountName = "John Doe 2", AccountNumber = "99736728", Balance = 0 };

        await _context.BankAccounts.AddAsync(bankAccount, _ctx);
        await _context.SaveChangesAsync(_ctx);

        var result = await _subject.GetAsync(id, _ctx);
        
        Assert.NotNull(result);
        Assert.Equal(result, (BankAccount)bankAccount);
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteItemAsExpected()
    {
        var id = Guid.NewGuid();
        var bankAccount = new BankAccountDto { Id= id, AccountName = "John Doe 3", AccountNumber = "11736728", Balance = 0 };

        await _context.BankAccounts.AddAsync(bankAccount, _ctx);
        await _context.SaveChangesAsync(_ctx);

        await _subject.DeleteAsync(id, _ctx);
        
        var result = await _context.BankAccounts.FirstOrDefaultAsync(x => x.Id == id, _ctx);
        
        Assert.Null(result);
    }
}