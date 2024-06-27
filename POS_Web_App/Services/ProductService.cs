using PosWebApp.Models;
using PosWebApp.Data;
namespace PosWebApp.Services
{
    public class ProductService
    {
        private readonly POSDbContext dbContext;

        public ProductService(POSDbContext context)
        {
            dbContext = context;
        }

        public void AddProduct(string name, decimal price, int quantity)
        {
            dbContext.Products.Add(new Product { Name = name, Price = price, Quantity = quantity });
            dbContext.SaveChanges();
        }

        public void UpdateProduct(string name, decimal price, int quantity)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.Name == name);
            if (product != null)
            {
                product.Price = price;
                product.Quantity = quantity;
                dbContext.SaveChanges();
            }
        }

        public void RemoveProduct(string name)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.Name == name);
            if (product != null)
            {
                dbContext.Products.Remove(product);
                dbContext.SaveChanges();
            }
        }

        public List<Product> GetAvailableProducts()
        {
            return dbContext.Products.Where(p => p.Quantity > 0).ToList();
        }

        public List<Product> ViewProducts()
        {
            return dbContext.Products.ToList();
        }
    }
}