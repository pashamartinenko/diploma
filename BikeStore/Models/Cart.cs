using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public IEnumerable<CartLine> Lines { get { return lineCollection; } }
        private DatabaseShopEntities1 db = new Models.DatabaseShopEntities1();
        public void AddItem(int productId, int quantity)
        {
            CartLine line = lineCollection.Find((e) => e.Product.Id == productId);
            if(line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                line = new CartLine
                {
                    Product = db.Bikes.FirstOrDefault(x => x.Id == productId),
                    Quantity = 1
                };
                lineCollection.Add(line);
            }
        }

        public void RemoveLine(Bikes product)
        {
            lineCollection.RemoveAll(l => l.Product.Id == product.Id);
        }
        public double? ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * Convert.ToDouble(e.Quantity));
        }
        public void Clear()
        {
            lineCollection.Clear();
        }

    }

    public class CartLine
    {
        public Bikes Product { get; set; }
        public int Quantity { get; set; }
    }
}
