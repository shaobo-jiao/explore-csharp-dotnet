using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;

public partial class Program
{
    public static void QueryCategories()
    {
        Console.WriteLine("Query categories and how many products they have");
        using NorthwindContext dbContext = new();

        IQueryable<Category>? categories = dbContext.Categories?
            .Include(c => c.Products);

        if (categories != null && categories.Any())
        {
            foreach (Category c in categories)
                Console.WriteLine($"{c.CategoryName} has {c.Products.Count()} products");
        }
    }

    public static void FilteredIncludes()
    {
        Console.WriteLine("Query categories and their products that has at least 100 units in stock");
        using NorthwindContext dbContext = new();

        IQueryable<Category>? categories = dbContext.Categories?
            .Include(c => c.Products.Where(p => p.Stock >= 100));

        if (categories != null && categories.Any())
        {
            foreach (Category c in categories)
            {
                Console.WriteLine($"{c.CategoryName} has {c.Products.Count()} products");
                foreach (Product p in c.Products)
                {
                    Console.WriteLine($"    {p.ProductName} has {p.Stock} units in stock");
                }
            }
        }
    }
    
    public static void QueryProducts()
    {
        Console.WriteLine("Products that cost more than a price, order by price desc:" + Environment.NewLine);
        using NorthwindContext dbContext = new();
        IQueryable<Product>? products = dbContext.Products?
            .Where(p => p.Cost >= 50)
            .OrderByDescending(p => p.Cost);
        Console.WriteLine(products?.ToQueryString());
        Console.WriteLine();
        if (products != null && products.Any())
        {
            foreach(Product p in products)
            {
                Console.WriteLine($"{p.ProductName} costs {p.Cost} and has {p.Stock} units in stock");
            }
        }
    }

}
