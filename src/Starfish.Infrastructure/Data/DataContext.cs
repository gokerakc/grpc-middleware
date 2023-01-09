using Microsoft.EntityFrameworkCore;
using Starfish.Infrastructure.DTOs;
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
        modelBuilder.Entity<BankTransactionDto>()
            .HasOne<BankAccountDto>()
            .WithMany()
            .HasForeignKey(x => x.SourceId)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<BankTransactionDto>()
            .HasOne<BankAccountDto>()
            .WithMany()
            .HasForeignKey(x => x.TargetId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        // Creates BankAccountsHistory table to track changes
        modelBuilder.Entity<BankAccountDto>()
            .ToTable(name: "BankAccounts", bankAccountsTable =>
            {
                bankAccountsTable.IsTemporal();
            });
        
        // Creates BankTransactionsHistory table to track changes
        modelBuilder.Entity<BankTransactionDto>()
            .ToTable(name: "BankTransactions", bankTransactionsTable =>
            {
                bankTransactionsTable.IsTemporal();
            });
    }

    public DbSet<BankAccountDto> BankAccounts { get; set; }
    
    public DbSet<BankTransactionDto> BankTransactions { get; set; }
    
    public DbSet<StarfishSettings> StarfishSettings { get; set; }

}