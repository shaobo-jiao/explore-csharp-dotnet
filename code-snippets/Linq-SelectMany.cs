using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Nutshell.EntityModels;

// SelectMany - Expanding and Flattening Subsequences

string[] fullNames = { "Anne Williams", "John Fred Smith", "Sue Green" };

// Fluent syntax
IEnumerable<string> query1 = fullNames.SelectMany(x => x.Split());
foreach (string name in query1)
    Console.Write(name + ", ");
Console.WriteLine();

// query syntax
IEnumerable<string> query2 =
    from fullName in fullNames
    from name in fullName.Split()
    select name;
foreach (string name in query2)
    Console.Write(name + ", ");
Console.WriteLine();

Console.WriteLine("-----------------------------");

// SelectMany - multiple range variables where query syntax is clearer

// query syntax
IEnumerable<string> query3 =
    from fullName in fullNames
    from name in fullName.Split()
    orderby fullName, name
    select $"{name} came from {fullName}";
foreach (string name in query3)
    Console.WriteLine(name + ", ");
Console.WriteLine();

// Fluent syntax
IEnumerable<string> query4 = fullNames
    .SelectMany(fullName => fullName.Split()
        .Select(name => new { FullName = fullName, Name = name }))
    .OrderBy(x => x.FullName)
    .ThenBy(x => x.Name)
    .Select(x => $"{x.Name} came from {x.FullName}");
foreach (string name in query4)
    Console.WriteLine(name + ", ");
Console.WriteLine();

Console.WriteLine("-----------------------------");

// SelectMany - JOINS

int[] numbers = [1,2,3];
string[] letters = ["a", "b"];

// Cartesian Joins: Output = 1a, 1b, 2a, 2b, 3a, 3b
IEnumerable<string> query5 = 
    from n in numbers
    from l in letters
    select n.ToString() + l;

// fluent syntax
IEnumerable<string> query6 = 
    numbers.SelectMany(n => letters.Select(l => new {n, l}))
    .Select(x => x.n.ToString() + x.l);

IEnumerable<string> query7 =
    numbers.SelectMany(n => letters.Select(l => n.ToString() + l));

// SelectMany - Joins in EF Core
using NutshellContext dbContext = new();

// Cartesian Join
var query8 = 
    from c in dbContext.Customers
    from p in dbContext.Purchases
    select c.Name + " might have bought a " + p.Description;

// Inner Join
var query9 = 
    from c in dbContext.Customers
    from p in dbContext.Purchases
    where c.ID == p.CustomerID
    select c.Name + " bought a " + p.Description;

// Inner Join with navigation property
var query10 = 
    from c in dbContext.Customers
    from p in c.Purchases
    select c.Name + " bought a " + p.Description;

query10 = dbContext.Customers
    .SelectMany(c => c.Purchases.Select(p => new {c, p}))
    .Select(cp => cp.c.Name + " bought a " + cp.p.Description);

query10 = dbContext.Customers
    .SelectMany(c => c.Purchases.Select(p => c.Name + " bought a " + p.Description));

// Left Outer Join

// OK in EFCore but crash in local as NullReferenceException (SQLite: APPLY not supported)
var query11 = 
    from c in dbContext.Customers
    from p in c.Purchases.DefaultIfEmpty()
    select new { c.Name, p.Description, Price = (decimal?) p.Price }; 

// OK in local as well as null handled
var query12 = 
    from c in dbContext.Customers
    from p in c.Purchases.DefaultIfEmpty()
    select new {
        CustomerName = c.Name, 
        PurchaseDescription = p == null ? null : p.Description, 
        PurchasePrice = p == null ? (decimal?)null : p.Price};

// add extra filtering on price
var query13 = 
    from c in dbContext.Customers
    from p in c.Purchases.Where(p => p.Price >= 1000).DefaultIfEmpty()
    select new {
        CustomerName = c.Name, 
        PurchaseDescription = p == null ? null : p.Description, 
        PurchasePrice = p == null ? (decimal?)null : p.Price};

