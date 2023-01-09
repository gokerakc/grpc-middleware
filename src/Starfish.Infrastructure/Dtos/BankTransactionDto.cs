using Starfish.Core.Models;

namespace Starfish.Infrastructure.DTOs;

public class BankTransactionDto
{
    public Guid Id { get; set; }
    
    public Guid SourceId { get; set; }
    
    public Guid TargetId { get; set; }
    
    public decimal Amount { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime Date { get; set; }
    
    public static BankTransactionDto FromBankTransaction(BankTransaction transaction)
    {
        return new BankTransactionDto
        {
            Id = transaction.Id,
            SourceId = transaction.SourceId,
            TargetId = transaction.TargetId,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Date = transaction.Date
        };
    }
    
    public static explicit operator BankTransaction(BankTransactionDto transaction)
    {
        return new BankTransaction
        {
            Id = transaction.Id,
            SourceId = transaction.SourceId,
            TargetId = transaction.TargetId,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Date = transaction.Date
        };
    }
}