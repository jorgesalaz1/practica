using BLL.Implementaciones;
using BLL.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Net;

namespace ProductService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteProductoController : ControllerBase
    {
        private readonly IClienteProductoService _clienteProductoService;
        public ClienteProductoController(IClienteProductoService clienteProductoService)
        {
            _clienteProductoService = clienteProductoService;
        }

        [HttpGet]
        public ActionResult<List<ClienteProducto>> GetClientProducts()
        {
            var clientesProductos = _clienteProductoService.GetAll();
            return Ok(clientesProductos);
        }

        [HttpGet("{id}")]
        public ActionResult<ClienteProducto> GetClientProducto(int id)
        {
            var clienteProducto =_clienteProductoService.GetById(id);

            if (clienteProducto == null)
            {
                return NotFound();
            }

            return Ok(clienteProducto);
        }

        [HttpPost]
        public ActionResult<ClienteProducto> PostClientProducto(ClienteProducto clienteProducto)
        {
            var nuevoClienteProducto = _clienteProductoService.Create(clienteProducto);

            return CreatedAtAction(
                nameof(GetClientProducto),
                new { id = nuevoClienteProducto.Id },
                nuevoClienteProducto
            );
        }
    }
}
