namespace Starfish.Core.Models;

public class BankTransaction
{
    public Guid Id { get; set; }
    
    public string Source { get; set; } = null!;
    
    public string Target { get; set; } = null!;
    
    public decimal Amount { get; set; }
    
    public string? Description { get; set; }
    
    public BankTransactionTypes Type { get; set; }
    
    public DateTime Date { get; set; }
}