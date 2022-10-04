using Library.ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApplication.API.Database;
using ShoppingCartApplication.API.EC;

namespace ShoppingCartApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuantityController : ControllerBase
    {
        private readonly ILogger<QuantityController> _logger;

        public QuantityController(ILogger<QuantityController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<ProductByQuantity> Get()
        {
            return new QuantityEC().Get();
        }
        [HttpPost("AddOrUpdate")]
        public ProductByQuantity AddOrUpdate(ProductByQuantity pq)
        {
            if (!FakeDatabase.Inventory.Any(p => p.ID == pq.ID))
            {
                pq.ID = FakeDatabase.NextId();
                FakeDatabase.QuantityProducts.Add(pq);
            }
            var productToUpdate = FakeDatabase.QuantityProducts.FirstOrDefault(p => p.ID == pq.ID);
            if (productToUpdate != null)
            {
                FakeDatabase.QuantityProducts.Remove(productToUpdate);
                FakeDatabase.QuantityProducts.Add(pq);
            }
            return pq;
        }

        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            var productToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.ID == id);
            if (productToDelete != null)
            {
                var product = productToDelete as ProductByQuantity;
                if (product != null)
                {
                    FakeDatabase.QuantityProducts.Remove(product);
                }

            }

            return id;
        }
    }
}
