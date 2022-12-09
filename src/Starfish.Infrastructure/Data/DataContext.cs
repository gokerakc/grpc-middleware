using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;

namespace Starfish.Infrastructure.Data;

public class DataContext : DbContext 
{
    public DataContext()
    {
    }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This is for local migrations
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=starfish-db;User Id=sa;Password=P455w0rd; TrustServerCertificate=True");
        }
    }
    
    public DbSet<BankAccount> BankAccounts { get; set; }
    
    public DbSet<BankTransaction> BankTransactions { get; set; }
}