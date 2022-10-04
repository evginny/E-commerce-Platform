using Library.ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApplication.API.EC;

namespace ShoppingCartApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return new InventoryEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product prod)
        {
            return new InventoryEC().AddOrUpdate(prod);
        }

        [HttpGet("Delete/{id}")]
        public bool Delete(int id)
        {
            return new InventoryEC().Delete(id);
        }

        [HttpPost("DeleteUnit")]
        public int RemoveUnit(Product prod)
        {
            return new InventoryEC().RemoveUnits(prod);
        }

        [HttpPost("DeleteWeight")]
        public int RemoveWeight(Product prod)
        {
            return new InventoryEC().RemoveWeight(prod);
        }

        [HttpPost("AddUnits")]
        public int AddUnits(Product prod)
        {
            return new InventoryEC().AddUnits(prod);
        }

        [HttpPost("AddWeight")]
        public int AddWeight(Product prod)
        {
            return new InventoryEC().AddWeight(prod);
        }
    }
}
