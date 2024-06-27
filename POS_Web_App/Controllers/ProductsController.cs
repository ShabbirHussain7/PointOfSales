using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models;
using PosWebApp.Services;

namespace PosWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAvailableProducts()
        {
            return Ok(_productService.GetAvailableProducts());
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _productService.AddProduct(product.Name, product.Price, product.Quantity);
            return CreatedAtAction(nameof(GetAvailableProducts), new { id = product.Id }, product);
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            _productService.UpdateProduct(product.Name, product.Price, product.Quantity);
            return NoContent();
        }

        [HttpDelete("{name}")]
        public IActionResult RemoveProduct(string name)
        {
            _productService.RemoveProduct(name);
            return NoContent();
        }
    }
}