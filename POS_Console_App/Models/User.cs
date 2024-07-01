using System.ComponentModel.DataAnnotations;

namespace POS.Models
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
}