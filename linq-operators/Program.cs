using Microsoft.EntityFrameworkCore;
using Nutshell.EntityModels;

using NutshellContext context = new();
context.Purchases.ExecuteDelete();