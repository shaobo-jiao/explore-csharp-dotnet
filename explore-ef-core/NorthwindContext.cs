using Microsoft.EntityFrameworkCore;

namespace Northwind.EntityModels;

public class NorthwindContext : DbContext
{
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(Environment.CurrentDirectory, "datasource", "Northwind.db");
        string connectionString = $"Data Source={dbPath}";
        optionsBuilder.UseSqlite(connectionString);
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        if (Database.ProviderName?.Contains("Sqlite") ?? false)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasConversion<double>();
        }
    }
}
