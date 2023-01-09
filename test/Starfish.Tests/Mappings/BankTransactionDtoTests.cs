using Starfish.Core.Models;
using Starfish.Infrastructure.DTOs;

namespace Starfish.Tests.Mappings;

public class BankTransactionDtoTests
{
    [Fact]
    public void ShouldMapToDtoAsExpected()
    {
        var bankTransaction = new BankTransaction
        {
            Id = Guid.NewGuid(),
            SourceId = Guid.NewGuid(),
            TargetId = Guid.NewGuid(),
            Amount = 550.00m,
            Description = "Desc",
            Date = DateTime.UtcNow
        };

        var dto = BankTransactionDto.FromBankTransaction(bankTransaction);
        
        Assert.Equal(bankTransaction.Id, dto.Id);
        Assert.Equal(bankTransaction.SourceId, dto.SourceId);
        Assert.Equal(bankTransaction.TargetId, dto.TargetId);
        Assert.Equal(bankTransaction.Amount, dto.Amount);
        Assert.Equal(bankTransaction.Description, dto.Description);
        Assert.Equal(bankTransaction.Date, dto.Date);
    }
    
    [Fact]
    public void ShouldMapToBankTransactionAsExpected()
    {
        var dto = new BankTransactionDto
        {
            Id = Guid.NewGuid(),
            SourceId = Guid.NewGuid(),
            TargetId = Guid.NewGuid(),
            Amount = 550.00m,
            Description = "Desc",
            Date = DateTime.UtcNow
        };

        var bankTransaction = (BankTransaction)dto;
        
        Assert.Equal(dto.Id, bankTransaction.Id);
        Assert.Equal(dto.SourceId, bankTransaction.SourceId);
        Assert.Equal(dto.TargetId, bankTransaction.TargetId);
        Assert.Equal(dto.Amount, bankTransaction.Amount);
        Assert.Equal(dto.Description, bankTransaction.Description);
        Assert.Equal(dto.Date, bankTransaction.Date);
    }
}