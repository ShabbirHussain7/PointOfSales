using POS.Models;
using Newtonsoft.Json;
using POS.Services;

namespace POS.Repositories
{
    public class AzureProductRepository : IProductRepository
    {
        private AzureStorageService _azureStorageService;
        private string containerName = "products";

        public AzureProductRepository(AzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        public void AddProduct(Product product)
        {
            var stream = SerializeProduct(product);
            _azureStorageService.UploadProductAsync(product.Id.ToString(), stream).Wait();
        }

        public void UpdateProduct(Product product)
        {
            AddProduct(product); // Overwrite the existing product
        }

        public void RemoveProduct(int id)
        {
            _azureStorageService.DeleteProductAsync(id.ToString()).Wait();
        }

        public List<Product> GetAvailableProducts()
        {
            var products = GetAllProducts();
            return products.Where(p => p.Quantity > 0).ToList();
        }

        public List<Product> GetAllProducts()
        {
            var blobs = _azureStorageService.ListProductsAsync().Result;
            return blobs.Select(blob => DeserializeProduct(blob.Content)).ToList();
        }

        private MemoryStream SerializeProduct(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return new MemoryStream(bytes);
        }

        private Product DeserializeProduct(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Product>(json);
            }
        }
    }
}