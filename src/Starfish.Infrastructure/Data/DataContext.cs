using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Shared;

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
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=starfish-db;User Id=sa;Password=P455w0rd; TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankTransaction>()
            .HasOne<BankAccount>()
            .WithMany()
            .HasForeignKey(x => x.SourceId)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<BankTransaction>()
            .HasOne<BankAccount>()
            .WithMany()
            .HasForeignKey(x => x.TargetId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        // Creates BankAccountsHistory table to track changes
        modelBuilder.Entity<BankAccount>()
            .ToTable(name: "BankAccounts", bankAccountsTable =>
            {
                bankAccountsTable.IsTemporal();
            });
        
        // Creates BankTransactionsHistory table to track changes
        modelBuilder.Entity<BankTransaction>()
            .ToTable(name: "BankTransactions", bankTransactionsTable =>
            {
                bankTransactionsTable.IsTemporal();
            });
    }

    public DbSet<BankAccount> BankAccounts { get; set; }
    
    public DbSet<BankTransaction> BankTransactions { get; set; }
    
    public DbSet<StarfishSettings> StarfishSettings { get; set; }

}