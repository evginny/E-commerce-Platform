using Library.ShoppingCart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Standard.Product.Utility
{
    public class ProductJsonConverter : JsonCreationConverter<Library.ShoppingCart.Models.Product>
    {
        protected override Library.ShoppingCart.Models.Product Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["isWeight"] != null || jObject["IsWeight"] != null)
            {
                return new ProductByWeight();
            }
            else if (jObject["isQuantity"] != null || jObject["IsQuantity"] != null)
            {
                return new ProductByQuantity();
            }
            else
            {
                return new Library.ShoppingCart.Models.Product();
            }
        }
    }
}
