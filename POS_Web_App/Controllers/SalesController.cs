using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models;
using PosWebApp.Services;

namespace PosWebApp.Controllers
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

        public IActionResult AddProductToSale(string productName, int quantity)
        {
            _salesService.AddProductToSale(productName, quantity);
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
            _salesService.GenerateReceipt(sale);
            return Ok(sale);
        }
    }
}