using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
    public class Product
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public bool Discontinued { get; set; }
        public DateTime LastSale { get; set; }
        public string Name { get; set; }

        public static Expression<Func<Product, bool>> IsSelling()
        {
            return p => !p.Discontinued && p.LastSale > DateTime.Now.AddDays(-30);
        }

        public IQueryable<Product> FilterSortProducts(IQueryable<Product> input)
        {
            return from P in input
                   where P.ID == ID
                   orderby P.LastSale
                   select P;
        }
       
    }
}
