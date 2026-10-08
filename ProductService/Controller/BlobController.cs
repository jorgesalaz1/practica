using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {

        private readonly BlobStorageService blobStorageService;

        public BlobController(BlobStorageService _blobStorageService)
        {
            blobStorageService = _blobStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                return BadRequest("Debe seleccionar un archivo");
            }

            await using var stream = file.OpenReadStream();
            await blobStorageService.UploadAsync(file.FileName, stream);
            return Ok(new
            {
                message = "Archivo subido correctamente",
                file = file.FileName
            });

        }

    }
}
