using BLL.Servicios;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementaciones
{
    public class ClienteProductoService : IClienteProductoService
    {
        public readonly ProductContext _context;

        public ClienteProductoService(ProductContext productContext)
        {
            _context = productContext;
        }
        public ClienteProducto Create(ClienteProducto clienteProducto)
        {
            var clienteExistente = _context.Clientes.Any(c => c.Id == clienteProducto.ClienteId);
            if (!clienteExistente)
            {
                throw new("El Cliente no existe");
            }

            var productoExistente = _context.Productos.Any(p => p.Id == clienteProducto.ProductoId);
            if (!productoExistente)
            {
                throw new("El Producto no existe");
            }


            _context.ClientesProductos.Add(clienteProducto);
            _context.SaveChanges();
            return clienteProducto;      
        }

        public List<ClienteProducto> GetAll()
        {
            return _context.ClientesProductos.ToList();
        }

        public ClienteProducto? GetById(int id)
        {
            return _context.ClientesProductos.FirstOrDefault(cp => cp.Id == id);
        }
    }
}
