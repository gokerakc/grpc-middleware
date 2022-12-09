namespace Starfish.Core.Models;

public class BankTransaction
{
    public Guid Id { get; set; }
    
    public required string Source { get; set; }
    
    public required string Target { get; set; }
    
    public decimal Amount { get; set; }
    
    public string? Description { get; set; }
    
    public BankTransactionTypes Type { get; set; }
    
    public DateTime Date { get; set; }
}