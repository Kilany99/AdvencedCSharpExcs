using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AdvencedCSharpExcs
{
    #region
    //Requirements:
    //- Create a list of Product objects with(Id, Name, Price, Category, IsDiscontinued)
    //- Write the same query using both query syntax and method syntax to:
    //  * Find all active products over $50
    //  * Group them by category
    //  * Order by price descending
    //- Compare and contrast both approaches
    internal class LINQBasics
    {
        private readonly List<Product> products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 1200, Category = "Electronics", IsDiscontinued = false },
        new Product { Id = 2, Name = "Mouse", Price = 25, Category = "Electronics", IsDiscontinued = false },
        new Product { Id = 3, Name = "Desk", Price = 300, Category = "Furniture", IsDiscontinued = false },
        new Product { Id = 4, Name = "Old Phone", Price = 150, Category = "Electronics", IsDiscontinued = true },
        new Product { Id = 5, Name = "Chair", Price = 100, Category = "Furniture", IsDiscontinued = false }
    };
        public IEnumerable<(string Category, IOrderedEnumerable<Product> Products)> MethodSyntax()
        {
            return products
                .Where(p => p.Price > 50 && !p.IsDiscontinued)
                .GroupBy(p => p.Category)
                .OrderByDescending(g => g.Max(p => p.Price))
                .Select(grouped => (
                    Category: grouped.Key,
                    Products: grouped.OrderByDescending(p => p.Price)
                ));

        }

        public IEnumerable<(string Category, IOrderedEnumerable<Product> Products)> QuerySyntax()
        {
            return from p in products
                              where p.Price > 50 && !p.IsDiscontinued
                              group p by p.Category into grouped
                              orderby grouped.Max(p => p.Price) descending
                              select 
                              (
                                  Category : grouped.Key,
                                  Products : grouped.OrderByDescending(p => p.Price)

                              );

        }

    }

    public class Test
    {
        public void Main()
        {
            LINQBasics obj = new LINQBasics();
            var resultQ = obj.QuerySyntax();
            var resultM = obj.MethodSyntax();
            foreach(var(category,products) in resultM)
            {
                Console.WriteLine($"Category : {category}");
                foreach (var product in products)
                {
                    Console.WriteLine($"    Product: {product.Name}, Price: {product.Price}");
                }

                Console.WriteLine();
            }

            foreach (var (category, products) in resultQ)
            {
                Console.WriteLine($"Category : {category}");
                foreach (var product in products)
                {
                    Console.WriteLine($"    Product: {product.Name}, Price: {product.Price}");
                }

                Console.WriteLine();
            }

        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsDiscontinued { get; set; }
    }

    #endregion

    #region

    //    //Deferred Execution
    //    Requirements:
    //- Create a scenario that demonstrates deferred execution vs immediate execution
    //- Use a list of numbers that:
    //  * Filters even numbers
    //  * Multiplies each by 2
    //  * Takes the first 3 items
    //- Add Console.WriteLine in the filter to show when it's actually executed
    //- Demonstrate the difference between using ToList() and not using it
    internal class Deferred
    {
        private List<int> numbers = Enumerable.Range(1, 10).ToList();
        public IEnumerable<int> DeffExec()
        {
            return numbers.
                Where(n =>
                {
                    Console.WriteLine($"Filtring {n}");
                    return n % 2 == 0;

                })
                .Select(n =>
                {
                    Console.WriteLine($"Multiplying{n}");
                    return n * 2;
                })
                .Take(3);
        }

        public List<int> ImmedExec()
        {
            return numbers.
                Where(n =>
                {
                    Console.WriteLine($"Filtering {n}");
                    return n % 2 == 0;
                })
                .Select(n =>
                {
                    Console.WriteLine($"Multiplying{n}");
                    return n * 2;
                })
                .Take(3)
                .ToList();
        }

        public void TestMain()
        {
            Console.WriteLine("Query defined but not executed yet...");
            Console.WriteLine("Executing query first time:");
            var deferredQuery = DeffExec();
            var immediateResults = ImmedExec();
            foreach (int num in deferredQuery)
            {
                Console.WriteLine($"Result: {num}");
            }

            Console.WriteLine("\nExecuting query second time:");
            foreach (var num in deferredQuery)
            {
                Console.WriteLine($"Result: {num}");
            }

            Console.WriteLine("Accessing results:");
            foreach (var num in immediateResults)
            {
                Console.WriteLine($"Result: {num}");
            }
        }
    }

    #endregion


}
