using Microsoft.EntityFrameworkCore;

// // create and save two customer;
// using NutshellContext dbContext = new NutshellContext();
// dbContext.Customers.Add(new() {Name = "shaobo"});
// dbContext.Customers.Add(new() {Name = "Second"});
// dbContext.SaveChanges();

using NutshellContext dbContext = new();
Customer a = dbContext.Customers.Single(c => c.Name == "Shaobo");
a.Name = "shaobo";
// IEnumerable<Customer> customers = dbContext.Customers.AsEnumerable();
// foreach(Customer c in customers)
//     c.Name = c.Name + "2";

foreach(var e in dbContext.ChangeTracker.Entries())
{
    Console.WriteLine ($"{e.Entity.GetType().FullName} is {e.State}");
    foreach (var m in e.Members)
        Console.WriteLine ($" {m.Metadata.Name}: '{m.CurrentValue}' modified: {m.IsModified}");
}

public class Customer
{
 public int ID { get; set; } 
 public string Name { get; set; } = null!;
}

public class NutshellContext : DbContext
{
    public DbSet<Customer> Customers {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Join(Environment.CurrentDirectory, "database", "testDb.db");
        string connectionString = $"Data Source={dbPath}";
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity => 
        {
            entity.ToTable("Customer");
            entity.Property(e => e.Name)
                .HasColumnName("Full Name")
                .IsRequired();
        });
        
    }
}