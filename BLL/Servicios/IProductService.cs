using Model;

namespace BLL.Servicios
{
    public interface IProductService
    {
        List<Producto> GetAll();
        Producto? GetById(int id);
        Producto Create(Producto producto);
    }
}
