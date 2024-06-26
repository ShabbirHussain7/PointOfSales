using System.ComponentModel.DataAnnotations;

namespace POS
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

    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal TotalAmount { get; set; }
    }
}