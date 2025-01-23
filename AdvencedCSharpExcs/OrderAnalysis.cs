using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
   

    #region
    //Let Clauses and Into Keywords
    //    Requirements:
    //- Create a list of Orders with(OrderId, CustomerId, Items, TotalAmount)
    //- Write a query using let to:
    //  * Calculate shipping cost based on total amount
    //  * Apply discount based on customer tier
    //  * Create a new anonymous type with original and calculated fields
    //- Use into to continue the query after a group clause

    public class OrderAnalysis
    {
        private List<Order> _orders = new();

        public Dictionary<int, string> _customerTiers = [];
        public OrderAnalysis()
        {
            InitializeData();
        }
        public void MainTest()
        {
            var orderAnalysis = UsingLet();
            var ordersByCustomerTier = UsingClauses();
            foreach (var analysis in orderAnalysis)
            {
                Console.WriteLine($"Order {analysis.OrderId}:");
                Console.WriteLine($"  Original Amount: ${analysis.OriginalAmount}");
                Console.WriteLine($"  Shipping: ${analysis.ShippingCost}");
                Console.WriteLine($"  Discount: ${analysis.Discount}");
                Console.WriteLine($"  Final Amount: ${analysis.FinalAmount}");
            }

            foreach (var tierAnalysis in ordersByCustomerTier)
            {
                Console.WriteLine($"\nTier: {tierAnalysis.CustomerTier}");
                Console.WriteLine($"  Orders: {tierAnalysis.OrderCount}");
                Console.WriteLine($"  Avg Value: ${tierAnalysis.AverageOrderValue:F2}");
                Console.WriteLine($"  Total Revenue: ${tierAnalysis.TotalRevenue:F2}");
            }
        }
        public IEnumerable<OrderAnalysisModel> UsingLet()
        {
            return (
                from order in _orders
                let shippingCost = CalculateShipping(order.TotalAmount)
                let customerTier = _customerTiers[order.CustomerId]
                let discount = CalculateDiscount(order.TotalAmount, customerTier)
                select new OrderAnalysisModel
                {
                    OrderId = order.OrderId,
                    OriginalAmount = order.TotalAmount,
                    ShippingCost = shippingCost,
                    Discount = discount,
                    FinalAmount = order.TotalAmount - discount + shippingCost,
                }
                );


        }

        public IEnumerable<CustomerTierModel> UsingClauses()
        {
            return
            from order in _orders
            group order by _customerTiers[order.CustomerId] into tierGroup
            let avgOrderValue = tierGroup.Average(o => o.TotalAmount)
            where avgOrderValue > 200
            select new CustomerTierModel
            {
                CustomerTier = tierGroup.Key,
                OrderCount = tierGroup.Count(),
                AverageOrderValue = avgOrderValue,
                TotalRevenue = tierGroup.Sum(o => o.TotalAmount)
            };

        }
        private decimal CalculateShipping(decimal totalAmount)
        {
            return totalAmount switch
            {
                <= 50 => 15,
                > 50 and < 100 => 10,
                _ => 5,
            };
        }
        private decimal CalculateDiscount(decimal totalAmount, string customerTier)
        {
            return customerTier switch
            {
                "Gold" => totalAmount * 0.1m,
                "Silver" => totalAmount * 0.05m,
                _ => 0
            };
        }

        private void InitializeData()
        {
            _orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    CustomerId = 1001,
                    Items = new List<Order.OrderItem>
                    {
                        new Order.OrderItem { ProductId = 101, Quantity = 2, Price = 50 },
                        new Order.OrderItem { ProductId = 102, Quantity = 1, Price = 100 }
                    },
                                TotalAmount = 200 // Calculated as 2*50 + 1*100
                },
                new Order
                {
                    OrderId = 2,
                    CustomerId = 1002,
                    Items = new List<Order.OrderItem>
                    {
                        new Order.OrderItem { ProductId = 103, Quantity = 3, Price = 30 },
                        new Order.OrderItem { ProductId = 104, Quantity = 1, Price = 200 }
                    },
                    TotalAmount = 290 // 3*30 + 1*200
                },
                new Order
                {
                    OrderId = 3,
                    CustomerId = 1003,
                    Items = new List<Order.OrderItem>
                    {
                        new Order.OrderItem { ProductId = 105, Quantity = 1, Price = 500 },
                        new Order.OrderItem { ProductId = 106, Quantity = 5, Price = 20 }
                    },
                    TotalAmount = 600 // 1*500 + 5*20
                },
                new Order
                {
                    OrderId = 4,
                    CustomerId = 1004,
                    Items = new List<Order.OrderItem>
                    {
                        new Order.OrderItem { ProductId = 107, Quantity = 2, Price = 150 },
                        new Order.OrderItem { ProductId = 108, Quantity = 1, Price = 75 }
                    },
                    TotalAmount = 375 // 2*150 + 1*75
                }
            };
            _customerTiers = new Dictionary<int, string>
            {
                { 1001, "Gold" },    // Customer with ID 1 is Gold tier
                { 1002, "Silver" },  // Customer with ID 2 is Silver tier
                { 1003, "Gold" },    // Customer with ID 3 is Gold tier
                { 1004, "Silver" }   // Customer with ID 4 is Silver tier
            };

        }

    }
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public record OrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }


    }
    public class CustomerTierModel
    {
        public string CustomerTier { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal TotalRevenue { get; set; }

    }
    public class OrderAnalysisModel
    {
        public int OrderId { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
    }

    #endregion

}
