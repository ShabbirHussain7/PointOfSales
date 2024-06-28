using System.ComponentModel.DataAnnotations;

namespace POS.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal TotalAmount { get; set; }
    }
}