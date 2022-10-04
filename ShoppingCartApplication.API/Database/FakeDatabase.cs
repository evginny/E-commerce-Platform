
using Library.ShoppingCart.Models;

namespace ShoppingCartApplication.API.Database
{
    public static class FakeDatabase
    {
        public static List<Product> Inventory
        {
            get
            {
                var returnList = new List<Product>();
                QuantityProducts.ForEach(returnList.Add);
                WeightProducts.ForEach(returnList.Add);

                return returnList;
            }
        }
        public static List<ProductByQuantity> QuantityProducts = new List<ProductByQuantity>
        {
            new ProductByQuantity {Name = "Default Quantity", Description = "quant desc", Quantity = 100, Price = 10, IsBogo = true, ID = 1},
        };
        public static List<ProductByWeight> WeightProducts = new List<ProductByWeight>
        {
            new ProductByWeight {Name = "Default Weight", Description = "weight desc", Weight = 100.5, Price = 10, IsBogo = true, ID = 2},
        };


        public static Dictionary<string, List<Product>> Carts { get; set; } = new Dictionary<string, List<Product>>();

        public static int NextId()
        {
                if (!Inventory.Any())
                {
                    return 1;
                }
                return Inventory.Select(p => p.ID).Max() + 1;
        }
    }
}
