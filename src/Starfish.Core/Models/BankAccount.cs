namespace Starfish.Core.Models;

public class BankAccount
{
    public string AccountNumber { get; set; } = null!;
    
    public string AccountName { get; set; } = null!;
    
    public decimal Balance { get; set; }

    public List<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();
}