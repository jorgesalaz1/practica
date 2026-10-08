using Azure.Storage.Blobs;
using BLL.Servicios;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementaciones
{
    public class BlobService : IBlobService
    {
        public async Task<List<BlobFilterDto>> GetFilesFromContainerAsync(string connectionString, string containerName)
        {
            var fileList = new List<BlobFilterDto>();
            var containerClient = new BlobContainerClient(connectionString, containerName);
            if(!await containerClient.ExistsAsync())
            {
                return fileList;
            }
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var downloadResult = await blobClient.DownloadContentAsync();
                fileList.Add(new BlobFilterDto
                {
                    FileName = blobItem.Name,
                    Content = downloadResult.Value.Content.ToString(),
                    FileSize = blobItem.Properties.ContentLength,
                    LastModified = blobItem.Properties.LastModified,
                });
            }
            return fileList;
        }

        public async Task<bool> UploadFilesAsync(string connecctionString, string containerName, string fileName, Stream content)
        {
            var containerClient = new BlobContainerClient(connecctionString, containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(content,overwrite:true);
            return true;
        }

        public async Task<bool> ProccesFileAsync(string connecctionString, string sourceContainer, string destinationContainer, string fileName)
        {
            var sourceContainerClient =  new BlobContainerClient(connecctionString, sourceContainer);
            var sourceBlobContainer = sourceContainerClient.GetBlobClient(fileName);
            if (!await sourceBlobContainer.ExistsAsync())
            {
                return false;
            }

            var destinationContainerClient = new BlobContainerClient(connecctionString, destinationContainer);
            await destinationContainerClient.CreateIfNotExistsAsync();
            var destinationBlobContainer = destinationContainerClient.GetBlobClient(fileName);
            var downloadResult = await sourceBlobContainer.DownloadContentAsync();
            await destinationBlobContainer.UploadAsync(downloadResult.Value.Content,overwrite:true);
            return true;

        }
    }
}
