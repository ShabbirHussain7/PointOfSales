using Microsoft.EntityFrameworkCore;
using POS.DTOs;
using POS.Models;

namespace POS.Database
{
    public class POSDbContext(DbContextOptions<POSDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProductDetail> SaleProductDetails { get; set; }
    }
}
