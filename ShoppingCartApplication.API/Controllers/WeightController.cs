using Library.ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApplication.API.Database;

namespace ShoppingCartApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeightController : ControllerBase
    {
        private readonly ILogger<WeightController> _logger;

        public WeightController(ILogger<WeightController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public List<ProductByWeight> Get()
        {
            return FakeDatabase.WeightProducts;
        }

        [HttpPost]
        public ProductByWeight AddOrUpdate(ProductByWeight pw)
        {
            if (!FakeDatabase.Inventory.Any(p => p.ID == pw.ID))
            {
                pw.ID = FakeDatabase.NextId();
                FakeDatabase.WeightProducts.Add(pw);
            }
            var productToUpdate = FakeDatabase.WeightProducts.FirstOrDefault(p => p.ID == pw.ID);
            if (productToUpdate != null)
            {
                FakeDatabase.WeightProducts.Remove(productToUpdate);
                FakeDatabase.WeightProducts.Add(pw);
            }
            return pw;
        }

        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            var productToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.ID == id);
            if (productToDelete != null)
            {
                var product = productToDelete as ProductByWeight;
                if (product != null)
                {
                    FakeDatabase.WeightProducts.Remove(product);
                }

            }

            return id;
        }
    }
}