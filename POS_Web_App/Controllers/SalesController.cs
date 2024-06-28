using Microsoft.AspNetCore.Mvc;
using POS.Services;

namespace POS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly SalesService _salesService;

        public SalesController(SalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost]
        public IActionResult AddProductToSale(int productId, int quantity)
        {
            _salesService.AddProductToSale(productId, quantity);
            return Ok();
        }

        [HttpGet("{id}/receipt")]
        public IActionResult GenerateReceipt(int id)
        {
            var sale = _salesService.GetSaleById(id);
            if (sale == null)
            {
                return NotFound();
            }
            var receipt = _salesService.GenerateReceipt(sale);
            return Ok(new { receipt });
        }
    }
}