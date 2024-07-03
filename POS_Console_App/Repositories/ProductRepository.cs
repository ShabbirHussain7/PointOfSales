using POS.Database;
using POS.Models;
using System.Collections.Generic;
using System.Linq;

namespace POS.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly POSDbContext _dbContext;

        public ProductRepository(POSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
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

        public List<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }
    }
}