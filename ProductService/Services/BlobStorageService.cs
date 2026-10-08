using Azure.Storage.Blobs;
using System.Data.Common;

namespace ProductService.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration["BlobStorage:ConnectionString"];
            string containerName = configuration["BlobStorage:ContainerName"];
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task UploadAsync(string fileName , Stream fileStream)
        {
            await _containerClient.CreateIfNotExistsAsync();
            BlobClient blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

    }
}
