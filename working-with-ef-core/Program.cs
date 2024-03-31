using Northwind.EntityModels; // To use Northwind.

using NorthwindDb db = new();
Console.WriteLine($"Provider: {db.Database.ProviderName}");