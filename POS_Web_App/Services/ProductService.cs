using System.Collections.Generic;
using System.Linq;
using PosWebApp.Data;
using PosWebApp.Models;

namespace PosWebApp.Services
{
    public class ProductService
    {
        private readonly POSDbContext _dbContext;

        public ProductService(POSDbContext context)
        {
            _dbContext = context;
        }

        public void AddProduct(string name, decimal price, int quantity)
        {
            _dbContext.Products.Add(new Product { Name = name, Price = price, Quantity = quantity });
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(int id, decimal price, int quantity)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Price = price;
                product.Quantity = quantity;
                _dbContext.SaveChanges();
            }
        }

        public void RemoveProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
        }

        public List<Product> GetAvailableProducts()
        {
            return _dbContext.Products.Where(p => p.Quantity > 0).ToList();
        }

        public List<Product> ViewProducts()
        {
            return _dbContext.Products.ToList();
        }
    }
}