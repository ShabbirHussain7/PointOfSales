using Microsoft.EntityFrameworkCore;

namespace POS
{
    public class POSDbContext(DbContextOptions<POSDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}
