using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;

public partial class Program
{

    private static void QueryWithLike()
    {
        Console.WriteLine("Query with like");
        using NorthwindDb db = new();

        Console.Write("Enter part of a product name: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return;
        
        IQueryable<Product>? products = db.Products?.Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));
        Console.WriteLine("ToQueryString: {0}", products?.ToQueryString());

        if (products is null || !products.Any())
            return;

        foreach(Product p in products)
        {
            Console.WriteLine("{0} has {1} units in stock. Discontinued: {2}",
                p.ProductName, p.Stock, p.Discontinued);
        }
    }

    private static void QueryProducts()
    {
        Console.WriteLine("*** Products that cost more than a price, highest at top ***");

        using NorthwindDb db = new NorthwindDb();

        string? input;
        decimal price;
        do
        {
            Console.Write("Enter a product price: ");
            input = Console.ReadLine();
        } while (!decimal.TryParse(input, out price));

        IQueryable<Product>? products = db.Products?
            .Where(p => p.Cost > price)
            .OrderByDescending(p => p.Cost);

        Console.WriteLine("ToQueryString: {0}", products?.ToQueryString());

        if (products is null || !products.Any())
        {
            Console.WriteLine("No product found.");
            return;
        }

        foreach (Product p in products)
        {
            Console.WriteLine("{0}: {1} costs {2:$#,##0.00} and has {3} in stock.",
                p.ProductId, p.ProductName, p.Cost, p.Stock);
        }
    }

    private static void FilteredIncludes()
    {
        Console.WriteLine("Products with a minimum number of units in stock: ");

        using NorthwindDb db = new();

        string? input;
        int stock;
        do
        {
            Console.Write("Enter a minimum for units in stock: ");
            input = Console.ReadLine();
        } while (!int.TryParse(input, out stock));

        IQueryable<Category>? categories = db.Categories?
            .Include(c => c.Products.Where(p => p.Stock >= stock));

        Console.WriteLine("ToQueryString: {0}", categories?.ToQueryString());

        if (categories is null || !categories.Any())
        {
            Console.WriteLine("No Category found.");
            return;
        }

        foreach (Category c in categories)
        {
            Console.WriteLine("{0} has {1} products with a minimum {2} units in stock.",
                c.CategoryName, c.Products.Count, stock);

            foreach (Product p in c.Products)
            {
                Console.WriteLine($"    {p.ProductName} has {p.Stock} units in stock");
            }
        }
    }

    private static void QueryCategories()
    {
        using NorthwindDb db = new();
        Console.WriteLine("Categories and how many products they have: ");

        IQueryable<Category>? categories = db.Categories?.Include(c => c.Products);

        Console.WriteLine("ToQueryString: {0}", categories?.ToQueryString());

        if (categories is null || !categories.Any())
        {
            Console.WriteLine("No Category found.");
            return;
        }

        foreach (Category c in categories)
        {
            Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products");
        }


    }
}
