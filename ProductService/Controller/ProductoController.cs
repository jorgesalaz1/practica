using BLL.Implementaciones;
using BLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ProductService.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public ActionResult<List<Producto>> GetProducts()
        {
            var productos = _productoService.GetAll();

            return Ok(productos);
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> GetProduct(int id)
        {
            var producto = _productoService.GetById(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        public ActionResult<Producto> PostProduct(Producto producto)
        {
            var nuevoProducto = _productoService.Create(producto);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = nuevoProducto.Id },
                nuevoProducto
            );
        }

    }
}
