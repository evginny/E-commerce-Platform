using Library.ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApplication.API.Database;
using ShoppingCartApplication.API.EC;

namespace ShoppingCartApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{fileName}")]
        public List<Product> Get(string fileName)
        {
            return new CartEC().Get(fileName);
        }

        [HttpPost("Add/{fileName}")]
        public Product Add(string fileName, Product pq)
        {
            return new CartEC().Add(fileName, pq);
        }

        [HttpPost("Delete/{fileName}")]
        public Product Delete(string fileName, Product prod)
        {
            return new CartEC().Delete(fileName, prod);
        }

        [HttpGet("SaveCart/{fileName}")]
        public string SaveCart(string fileName)
        {
            return new CartEC().SaveCart(fileName);
        }

        [HttpGet("GetCartNames")]
        public List<string> ReturnCartNames()
        {
            return new CartEC().ReturnCartNames();
        }
    }
}
