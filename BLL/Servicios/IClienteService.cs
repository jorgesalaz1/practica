using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Servicios
{
    public interface IClienteService
    {
        List<Cliente> GetAll();
        Cliente? GetById(int id);
        Cliente Create(Cliente cliente);
    }
}
