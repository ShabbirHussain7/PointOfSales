using Microsoft.EntityFrameworkCore;
using POS.Models;

public class CosmosDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToContainer("products");
        base.OnModelCreating(modelBuilder);
    }
}