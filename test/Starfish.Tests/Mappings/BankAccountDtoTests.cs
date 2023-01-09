using Starfish.Core.Models;
using Starfish.Infrastructure.DTOs;

namespace Starfish.Tests.Mappings;

public class BankAccountDtoTests
{
    [Fact]
    public void ShouldMapToDtoAsExpected()
    {
        var bankAccount = new BankAccount
        {
            Id = Guid.NewGuid(),
            Balance = 550.00m,
            AccountName = "Account Name",
            AccountNumber = "11552244"
        };

        var dto = BankAccountDto.FromBankAccount(bankAccount);
        
        Assert.Equal(bankAccount.Id, dto.Id);
        Assert.Equal(bankAccount.Balance, dto.Balance);
        Assert.Equal(bankAccount.AccountName, dto.AccountName);
        Assert.Equal(bankAccount.AccountNumber, dto.AccountNumber);
    }
    
    [Fact]
    public void ShouldMapToBankAccountAsExpected()
    {
        var dto = new BankAccountDto
        {
            Id = Guid.NewGuid(),
            Balance = 550.00m,
            AccountName = "Account Name",
            AccountNumber = "11552244"
        };

        var bankAccount = (BankAccount)dto;
        
        Assert.Equal(dto.Id, bankAccount.Id);
        Assert.Equal(dto.Balance, bankAccount.Balance);
        Assert.Equal(dto.AccountName, bankAccount.AccountName);
        Assert.Equal(dto.AccountNumber, bankAccount.AccountNumber);
    }
}