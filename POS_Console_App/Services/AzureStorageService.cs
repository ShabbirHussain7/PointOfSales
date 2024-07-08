using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace POS.Services
{
    public class AzureStorageService
    {
        private BlobServiceClient _blobServiceClient;
        private string _containerName = "products";

        public AzureStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task UploadProductAsync(string productId, Stream productStream)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(productId);
            await blobClient.UploadAsync(productStream, new BlobHttpHeaders { ContentType = "application/json" });
        }

        public async Task DeleteProductAsync(string productId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(productId);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<BlobDownloadInfo>> ListProductsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobs = new List<BlobDownloadInfo>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var response = await blobClient.DownloadAsync();
                blobs.Add(response.Value);
            }
            return blobs;
        }
    }
}