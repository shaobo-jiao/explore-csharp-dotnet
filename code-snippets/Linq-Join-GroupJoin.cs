using Nutshell.EntityModels;

// JOIN (default INNER JOIN)

using NutshellContext dbContext = new();
Customer[] customers = dbContext.Customers.ToArray();
Purchase[] purchases = dbContext.Purchases.ToArray();

// Select - Inefficient in local as repeatedly enumerate over every inner element.
var query1 = 
    from c in customers
    from p in purchases
    where c.ID == p.CustomerID
    orderby p.Price
    select c.Name + " bought a " + p.Description;

// Join - efficient in local as keyed loopup of inner sequence
var query2 = 
    from c in customers
    join p in purchases on c.ID equals p.CustomerID
    orderby p.Price
    select c.Name + " bought a " + p.Description;

// Join - fluent syntax
var query3 = customers
    .Join(purchases, c => c.ID, p => p.CustomerID, (c, p) => new {c, p})
    .OrderBy(x => x.p.Price)
    .Select(x => x.c.Name + " bought a " + x.p.Description);


// GROUPJOIN (default Left Outer Join)

// Select - inefficient
var query4 = 
    from c in customers
    select new 
    {
        CustomerName = c.Name,
        Purchases = purchases.Where(p => p.CustomerID == c.ID)
    };

// GroupJoin (Left Outher Join)
var query5 = 
    from c in customers
    join p in purchases on c.ID equals p.CustomerID into custPurchases
    select new 
    {
        CustomerName = c.Name,
        Purchases = custPurchases
    };

// Group Join (Inner Join)
var query6 = 
    from c in customers
    join p in purchases on c.ID equals p.CustomerID into custPurchases
    where custPurchases.Any() // turn into an inner join
    select new 
    {
        CustomerName = c.Name,
        Purchases = custPurchases
    };

// Group Join (Left Outer Join + Flat result set)
var query7 = 
    from c in customers
    join p in purchases on c.ID equals p.CustomerID into custPurchases
    from cp in custPurchases.DefaultIfEmpty() // to Left Outer Join
    select new 
    {
        CustomerName = c.Name,
        PurchaseDescription = cp == null ? null : cp.Description,
        PurchasePrice = cp == null ? (decimal?)null : cp.Price,
    };

// Lookup + SelectMany => Join
ILookup<int?, Purchase> purchaseLookup = purchases.ToLookup(p => p.CustomerID);

var query8 = 
    from c in customers
    from p in purchaseLookup[c.ID]  //Inner Join
    select new {c.Name, p.Description, p.Price};

var query9 = 
    from c in customers
    from p in purchaseLookup[c.ID].DefaultIfEmpty() // Left Outer Join
    select new
    {
        CustomerName = c.Name,
        PurchaseDescription = p == null ? null : p.Description,
        PurchasePrice = p == null ? (decimal?)null : p.Price,
    };

// Lookup + Select => GroupJoin (Left Outer Join)
var query10 = 
    from c in customers
    select new 
    {
        CustomerName = c.Name,
        Purchases = purchaseLookup[c.ID]
    };