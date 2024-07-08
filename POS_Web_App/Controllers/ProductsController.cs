using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using POS.Models;
using POS.Services;

namespace POS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize()]
    [RequiredScope("API.Calls")]
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
            Console.WriteLine("adding product in controller: ", product.Name);
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
            return Ok(_productService.GetAllProducts());
        }
    }
}