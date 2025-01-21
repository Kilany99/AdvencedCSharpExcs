using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace AdvencedCSharpExcs
{
    internal class Events
    {
    }

    //    Requirements:
    //- Create a StockMarket class that monitors multiple Stock objects
    //- Each Stock should raise events when its price changes by more than 10%
    //- Implement multiple event subscribers (Console Logger, File Logger, Email Notifier)
    //- Include custom EventArgs with relevant stock information

    public class StockMarket
    {
        private List<Stock> lsit = new List<Stock>();
        public void AddStock(Stock stock)
        {
            lsit.Add(stock);
            stock.PriceChanged += HandlePriceChanged;
        }

        public void RemoveStock(Stock stock)
        {
            stock.PriceChanged -= HandlePriceChanged;
            lsit.Remove(stock);
        }
        protected void HandlePriceChanged(object sender,StockMarketChangedEventArgs e)
        {
            ConsoleLogger.Log(e);
            FileLogger.Log(e);
            EmailNotifier.Notify(e);
        }

    }

    public class Stock
    {
        private decimal _price;
        public int _count { get; }
        public event EventHandler<StockMarketChangedEventArgs> PriceChanged;
        public Stock(decimal initialPrice, int count)
        {
            _price = initialPrice;
            _count = count;
        }

        public decimal Price
        {
            get =>_price; 
            set {
                if (value == Price)
                    return;
                decimal oldPrice = _price;
                _price = value; 
                decimal percentageChange = Math.Abs((value - oldPrice)/oldPrice*100);
                if (percentageChange >= 10)
                    OnPriceChanged(new StockMarketChangedEventArgs(value, _count, DateTime.UtcNow));
            }
        }
        protected virtual void OnPriceChanged(StockMarketChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);
        }
    }

    public class StockMarketChangedEventArgs:EventArgs
    {
        public decimal price;
        public int itemsCount;
        public DateTime expiryDate;
        public StockMarketChangedEventArgs(decimal p,int count,DateTime exp)
        {
            price = p;
            itemsCount = count;
            expiryDate = exp;
        }
    }

    public class ConsoleLogger
    {
        public static void Log(StockMarketChangedEventArgs e)
        { 
            Console.WriteLine(e.ToString());
        }
    }

    public class FileLogger
    {
        public static void Log(StockMarketChangedEventArgs e)
        {
           
            File.AppendAllText("stocklog.txt",e.ToString());
        }
    }

    public class EmailNotifier
    {
        public static void Notify(StockMarketChangedEventArgs e)
        {
            //implement email service here
            throw new NotImplementedException();
        }
    }
}