using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Servicios
{
    public interface IClienteProductoService
    {
        List<ClienteProducto> GetAll();
        ClienteProducto? GetById(int id);
        ClienteProducto Create(ClienteProducto clienteProducto);
    }
}
