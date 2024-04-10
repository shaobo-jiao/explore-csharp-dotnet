using Microsoft.EntityFrameworkCore;

namespace Nutshell.EntityModels;

public class NutshellContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!; // null will never be observed
    public DbSet<Purchase> Purchases { get; set; } = null!; // null will never be observed

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(Environment.CurrentDirectory, "datasource", "nutshell.db");
        string connectionString = $"data source = {dbPath}";
        Console.WriteLine($"connection string: {connectionString}");

        optionsBuilder.UseSqlite(connectionString);

        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity => 
        {
            entity.ToTable("Customer");
            entity.Property(c => c.Name).IsRequired().HasMaxLength(30);
        });

        modelBuilder.Entity<Purchase>(entity => 
        {
            entity.ToTable("Purchase");
            entity.Property(p => p.Date).IsRequired();
            entity.Property(p => p.Description).IsRequired().HasMaxLength(30);
            entity.Property(p => p.Price).HasConversion<double>().HasColumnType("NUMERIC");
        });

    }
}
