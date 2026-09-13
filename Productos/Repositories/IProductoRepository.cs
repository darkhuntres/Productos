using Productos.Models;

namespace Productos.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<bool> TipoProductoExisteAsync(int tipoProductoId);
    Task AgregarAsync(Producto producto);
    void Actualizar(Producto producto);
    void Eliminar(Producto producto);
    Task<bool> GuardarCambiosAsync();
    Task<List<TipoProducto>> ObtenerTiposAsync();
}
