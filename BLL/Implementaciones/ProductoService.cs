using BLL.Servicios;
using DAL.Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementaciones
{
    public class ProductoService : IProductService

    {
        private readonly ProductContext _productContext;

        public ProductoService(ProductContext prductContext)
        {
            _productContext = prductContext;
        }
        public Producto Create(Producto producto)
        {
           _productContext.Productos.Add(producto);
           _productContext.SaveChanges();
            return producto;
        }

        public List<Producto> GetAll()
        {
            return _productContext.Productos.ToList();
        }

        public Producto? GetById(int id)
        {
           return _productContext.Productos.FirstOrDefault(p => p.Id == id);
        }
    }
}
