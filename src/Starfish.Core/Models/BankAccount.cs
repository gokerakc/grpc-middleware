namespace Starfish.Core.Models;

public class BankAccount
{
    public Guid Id { get; set; }
    
    public required string AccountNumber { get; set; }
    
    public required string AccountName { get; set; }
    
    public decimal Balance { get; set; }
}