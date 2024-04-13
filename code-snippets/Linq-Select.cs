using Nutshell.EntityModels;

// select subquery: list 1st lvl folders and their contained files;

string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
Console.WriteLine($"Path: {path}");
DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();

// query syntax
var query1 = 
    from d in dirs
    where (d.Attributes & FileAttributes.System) == 0
    select new {
        DirName = d.FullName,
        CreationTime = d.CreationTime,
        Files = from f in d.GetFiles()
            where (f.Attributes & FileAttributes.Hidden) == 0
            select new {FileName = f.Name, f.Length}
    };

foreach(var dirFiles in query1)
{
    Console.WriteLine($"Directory: {dirFiles.DirName}, created at {dirFiles.CreationTime}");
    foreach(var file in dirFiles.Files)
    {
        Console.WriteLine($"    {file.FileName}, {file.Length}");
    }
}
Console.WriteLine("-----------------------------------");

// Equivalent Fluent API
var query2 = dirs
    .Where(d => (d.Attributes & FileAttributes.System) == 0)
    .Select(d => new {
        DirName = d.FullName,
        CreationTime = d.CreationTime,
        Files = d.GetFiles()
            .Where(f => (f.Attributes & FileAttributes.Hidden) == 0)
            .Select(f => new {
                FileName = f.Name,
                Length = f.Length,
            }),
    });

foreach(var dirFiles in query2)
{
    Console.WriteLine($"Directory: {dirFiles.DirName}, created at {dirFiles.CreationTime}");
    foreach(var file in dirFiles.Files)
    {
        Console.WriteLine($"    {file.FileName}, {file.Length}");
    }
}
Console.WriteLine("-----------------------------------");


// select - INNER JOIN: list customers who has (>= 2) high-value purchases;

// query syntax
using (NutshellContext dbContext = new NutshellContext())
{
    var query3 = 
        from c in dbContext.Customers
        let highValuePurchases = 
            from p in c.Purchases
            where p.Price >= 1000
            select new {Description = p.Description, Price = p.Price,}
        where highValuePurchases.Any()
        select new {CustomerName = c.Name, HighValuePurchases = highValuePurchases};

    foreach(var customerPurchases in query3)
    {
        Console.WriteLine($"Customer {customerPurchases.CustomerName} has {customerPurchases.HighValuePurchases.Count()} purchases");
        foreach(var purchase in customerPurchases.HighValuePurchases)
        {
            Console.WriteLine($"    Purchase {purchase.Description}, price at {purchase.Price}");
        }
    }
    Console.WriteLine("-----------------------------------");
}

using (NutshellContext dbContext = new NutshellContext())
{
    var query4 = dbContext.Customers
        .Select(c => new {
            CustomerName = c.Name, 
            HighValuePurchases = c.Purchases
                .Where(p => p.Price >= 1000)
                .Select(p => new {Description = p.Description, Price = p.Price})  
        })
        .Where(cp => cp.HighValuePurchases.Count() >= 2);

    foreach(var customerPurchases in query4)
    {
        Console.WriteLine($"Customer {customerPurchases.CustomerName} has {customerPurchases.HighValuePurchases.Count()} purchases");
        foreach(var purchase in customerPurchases.HighValuePurchases)
        {
            Console.WriteLine($"    Purchase {purchase.Description}, price at {purchase.Price}");
        }
    }
    Console.WriteLine("-----------------------------------");
}