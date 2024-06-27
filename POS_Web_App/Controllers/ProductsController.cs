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

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            _productService.UpdateProduct(id, product.Price, product.Quantity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveProduct(int id)
        {
            _productService.RemoveProduct(id);
            return NoContent();
        }

        [HttpGet("all")]
        public IActionResult ViewProducts()
        {
            return Ok(_productService.ViewProducts());
        }
    }
}