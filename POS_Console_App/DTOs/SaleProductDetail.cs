using System.ComponentModel.DataAnnotations;

namespace POS.DTOs
{
    public class SaleProductDetail
    {
        [Key]
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string ProductName { get; set; }
        public int QuantityBought { get; set; }
        public decimal Price { get; set; }
    }

    
}