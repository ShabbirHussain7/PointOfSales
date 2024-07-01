using POS.Models;
using POS.Repositories;
using System.Collections.Generic;

namespace POS.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(string name, decimal price, int quantity)
        {
            var product = new Product { Name = name, Price = price, Quantity = quantity };
            _productRepository.AddProduct(product);
        }

        public void UpdateProduct(int id, decimal price, int quantity)
        {
            var product = new Product { Id = id, Price = price, Quantity = quantity };
            _productRepository.UpdateProduct(product);
        }

        public void RemoveProduct(int id)
        {
            _productRepository.RemoveProduct(id);
        }

        public List<Product> GetAvailableProducts()
        {
            return _productRepository.GetAvailableProducts();
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }
    }
}