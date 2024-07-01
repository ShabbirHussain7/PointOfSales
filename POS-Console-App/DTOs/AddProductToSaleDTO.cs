using System.Collections.Generic;

namespace POS.DTOs
{
    public class AddProductsToSaleDto
    {
        public List<ProductQuantityDto> Products { get; set; }
    }

    public class ProductQuantityDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}