using System.Collections.Generic;

namespace POS.DTOs
{
    public class ReceiptDto
    {
        public List<ReceiptProductDetailDto> ProductDetails { get; set; } = new List<ReceiptProductDetailDto>();
        public decimal TotalAmount { get; set; }
    }

    public class ReceiptProductDetailDto
    {
        public string ProductName { get; set; }
        public int QuantityBought { get; set; }
        public decimal Price { get; set; }
    }
}