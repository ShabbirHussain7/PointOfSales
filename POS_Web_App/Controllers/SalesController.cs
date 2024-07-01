using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.DTOs;
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

        [HttpPost("add")]
        public IActionResult AddProductsToSale([FromBody] AddProductsToSaleDto addProductsToSaleDto)
        {
            _salesService.AddProductsToSale(addProductsToSaleDto);
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
            return Ok(receipt);
        }
    }
}