using BLL.Implementaciones;
using BLL.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ProductService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> GetClients()
        {
            var clientes = _clienteService.GetAll();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> GetClient(int id)
        {
            var cliente = _clienteService.GetById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPost]
        public ActionResult<Producto> PostClient(Cliente cliente)
        {
            var nuevoCliente = _clienteService.Create(cliente);

            return CreatedAtAction(
                nameof(GetClient),
                new { id = nuevoCliente.Id },
                nuevoCliente
            );
        }
    }
}
