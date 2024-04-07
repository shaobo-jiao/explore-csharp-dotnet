using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;

Func<Product, bool> pred1 = p => p.Cost >= 50;
Expression<Func<Product, bool>> pred2 = p => p.Cost >= 50;
using NorthwindContext context = new();
IQueryable<Product>? products = context.Products?.Where(pred2);