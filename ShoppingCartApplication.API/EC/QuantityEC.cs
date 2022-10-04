using Library.ShoppingCart.Models;
using ShoppingCartApplication.API.Database;

namespace ShoppingCartApplication.API.EC
{
    public class QuantityEC
    {
        public List<ProductByQuantity> Get()
        {
            return FakeDatabase.QuantityProducts;
        }
    }
}
