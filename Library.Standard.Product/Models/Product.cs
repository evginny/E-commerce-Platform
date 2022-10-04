using Library.Standard.Product.Utility;
using Newtonsoft.Json;
using System;

namespace Library.ShoppingCart.Models
{
    [JsonConverter(typeof(ProductJsonConverter))]
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public virtual int Quantity { get; set; }
        public virtual double TotalPrice { get; set; }

        public virtual double Weight { get; set; }
        public virtual bool IsBogo { get; set; }
        public int ID { get; set; }

        public override string ToString()
        {
            return $"{ID} - {Name}: {Description}; {Math.Round(Price, 2)}\n";
        }
    }
}