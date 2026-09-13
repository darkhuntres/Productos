using Productos.DTOs.Productos;

namespace Productos.Services;

public interface IProductoService
{
    Task<List<ProductoDto>> ObtenerTodosAsync();
    Task<ProductoDto?> ObtenerPorIdAsync(int id);
    Task<(ProductoDto? Producto, string? Error)> CrearAsync(ProductoRequest request);
    Task<(ProductoDto? Producto, string? Error)> ActualizarAsync(int id, ProductoRequest request);
    Task<bool> EliminarAsync(int id);
    Task<List<TipoProductoDto>> ObtenerTiposAsync();
}
