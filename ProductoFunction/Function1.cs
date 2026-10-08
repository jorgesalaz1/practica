
using Azure.Storage.Blobs;
using BLL.Implementaciones;
using BLL.Servicios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ProductoFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IBlobService _iblobService;

        public Function1(ILogger<Function1> logger, IBlobService iblobService)
        {
            _logger = logger;
            _iblobService = iblobService;
        }

        //[Function("TimerProcessFiles")]
        //public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        //{
        //    _logger.LogInformation($"[Timer] Revisando el contenedor 'samples-workitems'...");
        //    string connectionString = System.Environment.GetEnvironmentVariable("ProductBlobTrigger");
        //    if (string.IsNullOrEmpty(connectionString)) return;
        //    var containerClient = new BlobContainerClient(connectionString, "samples-workitems");

        //    if (!await containerClient.ExistsAsync()) return;
        //    await foreach (var blobItem in containerClient.GetBlobsAsync())
        //    {
        //        var blobClient = containerClient.GetBlobClient(blobItem.Name);
        //        var downloadResult = await blobClient.DownloadContentAsync();
        //        string content = downloadResult.Value.Content.ToString();

        //        _logger.LogInformation($"Archivo encontrado: {blobItem.Name}");
        //        _logger.LogInformation($"Contenido: {content}");
        //    }
        //}

        [Function("GetFilesApi")]
        public async Task<HttpResponseData> RunHttp([HttpTrigger(AuthorizationLevel.Anonymous,
                                                 "get", Route="files")] HttpRequestData req) 
        {
            _logger.LogInformation("Solicitud GET HTTP recibida en ProductoFunction");
            string connection = Environment.GetEnvironmentVariable("ProductBlobTrigger");
            if (string.IsNullOrEmpty(connection))
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("La cadena de conexión 'ProductBlobTrigger' no está configurada.");
                return errorResponse;
            }
           
            var files  = await _iblobService.GetFilesFromContainerAsync(connection, "samples-workitems");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(files);
            return response;
        }

        [Function("UploadFilesApi")]
        public async Task<HttpResponseData> Upload([HttpTrigger(AuthorizationLevel.Anonymous,
                                                 "post", Route="files")] HttpRequestData req)
        {
            _logger.LogInformation("Solicitud POST HTTP recibida en ProductoFunction");
            string connection = Environment.GetEnvironmentVariable("ProductBlobTrigger");
            string fileName = req.Query["filename"];
            if (string.IsNullOrEmpty(fileName)) 
            { 
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("\"Por favor indica el nombre del archivo " +
                                                    "en el parámetro URL, ej: /api/files?filename=cliente.txt");
                return badResponse;
            }
            await _iblobService.UploadFilesAsync(connection,"samples-workitems", fileName, req.Body);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(fileName);
            return response;
        }

        [Function("ProccessedFileApi")]
        public async Task<HttpResponseData> ProcessFile([HttpTrigger(AuthorizationLevel.Anonymous,"post",
                                                           Route = "files/process")] HttpRequestData req)
        {
            string correlationId = req.Headers.TryGetValues("X-CorrelationId",out var values) ? 
                                   values.FirstOrDefault() ?? Guid.NewGuid().ToString() 
                                   : Guid.NewGuid().ToString();
            using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId}))
            {
                _logger.LogInformation("Procesando archivo...");
                string connection = Environment.GetEnvironmentVariable("ProductBlobTrigger");
                string fileName = req.Query["filename"];
                _logger.LogInformation("[INIT] [{CorrelationId}] Iniciando solicitud de procesamiento.", correlationId);
                if (string.IsNullOrEmpty(fileName))
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Indique el nombre del archivo");
                    return badResponse;
                }
                _logger.LogInformation("[PROCESSING] [{CorrelationId}] Moviendo '{FileName}' a 'samples-processed'...", correlationId, fileName);
                bool success = await _iblobService.ProccesFileAsync(connection, "samples-workitems", "samples-processed", fileName);
                if (!success)
                {
                    _logger.LogError("[BLOB_NOT_FOUND] [{CorrelationId}] El archivo '{FileName}' no existe en origen.", correlationId, fileName);
                    var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                    await notFoundResponse.WriteStringAsync($"El archivo '{fileName}' no fue encontrado en el contenedor de origen.");
                    return notFoundResponse;
                }
                _logger.LogInformation("[SUCCESS] [{CorrelationId}] Archivo '{FileName}' procesado correctamente.", correlationId, fileName);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("X-CorrelationId", correlationId);
                await response.WriteStringAsync($"El archivo '{fileName}' fue procesado y movido exitosamente a 'samples-processed'.");
                return response;

            }     
        }

        [Function("GetProcessedFileApi")]
        public async Task<HttpResponseData> GetProcessedFiles([HttpTrigger(AuthorizationLevel.Anonymous, "get",
                                                                Route = "files/proccessed")] HttpRequestData req)
        {
            _logger.LogInformation("Consultando archivos procesados....");
            string connection = Environment.GetEnvironmentVariable("ProductBlobTrigger");
            var files = await _iblobService.GetFilesFromContainerAsync(connection, "samples-processed");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(files);
            return response;
        }


    }
}