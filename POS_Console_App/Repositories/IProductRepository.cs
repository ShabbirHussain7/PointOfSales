using POS.Models;
using System.Collections.Generic;

namespace POS.Repositories
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(int id);
        List<Product> GetAvailableProducts();
        List<Product> GetAllProducts();
    }
}