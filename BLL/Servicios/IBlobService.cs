using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Servicios
{
    public interface IBlobService
    {
        Task<List<BlobFilterDto>> GetFilesFromContainerAsync(string connectionString, string containerName);
        Task<bool> UploadFilesAsync(string connecctionString, string containerName, string fileName, Stream content);
        Task<bool> ProccesFileAsync(string connecctionString, string sourceContainer, string destinationContainer, string fileName);
    }
}
