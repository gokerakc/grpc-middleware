namespace Starfish.Core.Models;

public record BankTransaction
{
    public Guid Id { get; set; }
    
    public Guid SourceId { get; set; }
    
    public Guid TargetId { get; set; }
    
    public decimal Amount { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime Date { get; set; }
}