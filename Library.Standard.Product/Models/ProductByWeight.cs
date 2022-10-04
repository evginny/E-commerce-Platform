using Library.Standard.Product.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ShoppingCart.Models
{
    [JsonConverter(typeof(ProductJsonConverter))]
    public class ProductByWeight : Product
    {
        public override double Weight{ get; set; }
        public override double TotalPrice
        {
            get
            {
                if (IsBogo)
                {
                    if ((Math.Floor(Weight) % 2 == 0))
                    {
                        return (Price * (Math.Floor(Weight) / 2 + (Weight % 2)));
                    }
                    else if ((Math.Floor(Weight) - 1) % 2 == 0)
                    {
                        if ((Math.Floor(Weight) - 1) == 0)
                        {
                            return (Weight - 1) * Price;
                        }
                        return (Price * ((Math.Floor(Weight) - 1) / 2 + ((Weight - 1) % 2) + 1));
                    }
                    else
                    {
                        return Price;
                    }
                }
                else
                {
                    return Weight * Price;
                }
            }
        }
        public override bool IsBogo { get; set; }
        public bool IsWeight { get; set; }
        public ProductByWeight()
        {

        }
        public ProductByWeight(Product product)
        {
            this.ID = product.ID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Price = product.Price;
            this.Weight = product.Weight;
            this.IsBogo = product.IsBogo;
        }


        public override string ToString()
        {
            return $"{ID} - {Name}: {Description};" +
                $" {Math.Round(Price, 2)} x {Math.Round(Weight, 1)} pounds = {Math.Round(TotalPrice, 2)};" +
                $" \"bogo\": {IsBogo}\n";
        }
    }
}
