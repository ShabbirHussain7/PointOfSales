using System.ComponentModel.DataAnnotations;
using POS.DTOs;

namespace POS.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public List<SaleProductDetail> ProductDetails { get; set; } = new List<SaleProductDetail>();
        public decimal TotalAmount { get; set; }
    }
}