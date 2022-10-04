using Library.ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApplication.API.Database;

namespace ShoppingCartApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public List<Product> Get()
        {
            return FakeDatabase.Inventory;
        }
    }

}
