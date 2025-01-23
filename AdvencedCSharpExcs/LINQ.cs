using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
    internal class LINQ
    {
        public IEnumerable<string> Selector(string[] names)
        {
            IEnumerable<string> list = names.
                Where(n => n.Contains("a")).
                OrderBy(n => n.Length).
                Select(n => n.ToUpper());
            foreach (string name in list)
                yield return name;
        }
        public IEnumerable<string> Selector2(string[] names)  // fluent-syntax query
        {
            IEnumerable<string> query =
            from n in names
            where n.Contains("a") // Filter elements
            orderby n.Length // Sort elements
            select n.ToUpper(); // Translate each element (project)
            foreach (string name in query) 
                yield return name;
        }
        public IEnumerable<TempProjectionItem> TempVowel(string[] names)
        {
            return from n in names
            select new TempProjectionItem
            {
                Original = n,
                Vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "")
            .Replace("o", "").Replace("u", "")
            };
        }

        public IEnumerable<string> AnonymousTypeVowel(string[] names)
        {
            var intermediate =
                from n in names
                select new
                {
                    Original = n,
                    Vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "")
                .Replace("o", "").Replace("u", "")
                };
            return from item in intermediate
                   where item.Vowelless.Length > 2
                   select item.Original;
        }

        public IEnumerable<string> LetVowel(string[] names)
        {
            
            return from n in names
            let vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "")
            .Replace("o", "").Replace("u", "")
            where vowelless.Length > 2
            orderby vowelless
            select n; // Thanks to let, n is still in scope.
        }

        public IQueryable<string> SQLDB(string[] names)
        {
            IEnumerable<string> query =
            from n in names
            select n.Replace("a", "").Replace("e", "").Replace("i", "")
            .Replace("o", "").Replace("u", "")
                                        into noVowel
                                        where noVowel.Length >= 3
                                        orderby noVowel
                                        select noVowel;
            foreach (string s in query) Console.WriteLine(s + "|");

            using var dbContext = new NutshellContext();
             return from c in dbContext.Customers
                                        where c.Name.Contains("a")
                                        orderby c.Name.Length
                                        select c.Name.ToUpper();
        }
    }

    class TempProjectionItem
    {
        public string Original; // Original name
        public string Vowelless; // Vowel-stripped name
        public TempProjectionItem()
        {
            Original ="";
            Vowelless = "";
        }
    }



    public class Customer
    {
        public int ID { get; set; }
        public string? Name { get; set; }
    }

    public class Purchase
    {
        public int ID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual Customer Customer { get; set; }
    }
    public class NutshellContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        => builder.UseSqlServer("...connection string...");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.Property(e => e.Name).IsRequired(); // Column is not nullable
            });
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });
        }
    }
   

    public class MyMainClass
    {
        private string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

        public void MyMainFunc()
        {
            LINQ obj = new();
            string[] values = obj.Selector(names).ToArray();
            foreach (string a in values)
                Console.WriteLine(a + " ");

            IEnumerable<string> query = names.Where(name => name.EndsWith("y"));// Harry // Mary // Jay

            IEnumerable<string> q = names.Where((n, i) => i % 2 == 0);// Tom// Harry// Jay

            int[] numbers = { 3, 5, 2, 234, 4, 1 };
            var takeWhileSmall = numbers.TakeWhile(n => n < 100); // { 3, 5, 2 }

            var skipWhileSmall = numbers.SkipWhile(n => n < 100); // { 234, 4, 1 }
            string tempPath = Path.GetTempPath();
            DirectoryInfo[] dirs = new DirectoryInfo(tempPath).GetDirectories();
            var query2 =
            from d in dirs
            where (d.Attributes & FileAttributes.System) == 0
            select new
            {
                DirectoryName = d.FullName,
                Created = d.CreationTime,
                Files = from f in d.GetFiles()
                        where (f.Attributes & FileAttributes.Hidden) == 0
                        select new { FileName = f.Name, f.Length, }
            };
            foreach (var dirFiles in query2)
            {
                Console.WriteLine("Directory: " + dirFiles.DirectoryName);
                foreach (var file in dirFiles.Files)
                    Console.WriteLine(" " + file.FileName + " Len: " + file.Length);
            }

        }
    }

}
