using Starfish.Core.Models;

namespace Starfish.Infrastructure.DTOs;

public class BankAccountDto
{
    public Guid Id { get; set; }
    
    public required string AccountNumber { get; set; }
    
    public required string AccountName { get; set; }
    
    public decimal Balance { get; set; }

    public static BankAccountDto FromBankAccount(BankAccount account)
    {
        return new BankAccountDto
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            Balance = account.Balance
        };
    }
    
    public static explicit operator BankAccount(BankAccountDto account)
    {
        return new BankAccount
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            Balance = account.Balance
        };
    }
}