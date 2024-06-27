using System.ComponentModel.DataAnnotations;

namespace PosWebApp.Models
{
    public enum UserRole { Admin, Cashier }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class SaleProductDetail
    {
        [Key]
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string ProductName { get; set; }
        public int QuantityBought { get; set; }
        public decimal Price { get; set; }
    }

    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public List<SaleProductDetail> ProductDetails { get; set; } = new List<SaleProductDetail>();
        public decimal TotalAmount { get; set; }
    }
}