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
    public class ProductByQuantity : Product
    {
        public override bool IsBogo { get; set; }    
        public override int Quantity { get; set; }
        public bool IsQuantity { get; set; }
        public override double TotalPrice
        {
            get
            {
                if (IsBogo)
                {
                    if (Quantity % 2 == 0)
                    {
                        return Price / 2 * Quantity;
                    }
                    else if ((Quantity % 2 == 1) && (Quantity > 1))
                    {
                        return (Price / 2 * (Quantity - 1) + Price);
                    }
                    else
                    {
                        return Price;
                    }
                }
                else
                {
                    return Price * Quantity;
                }
            }
        }
        public ProductByQuantity()
        {

        }
        public ProductByQuantity(Product product)
        {
            this.ID = product.ID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Price = product.Price;
            this.Quantity = product.Quantity;
            this.IsBogo = product.IsBogo;
        }

        public override string ToString()
        {
            return $"{ID} - {Name}: {Description};" +
                $" ${Math.Round(Price, 2)} x {Quantity} unit(s) = {Math.Round(TotalPrice, 2)};" +
                $" bogo: {IsBogo}\n";
        }
        
    }
}
