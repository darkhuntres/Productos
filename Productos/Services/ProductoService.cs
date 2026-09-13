using Productos.DTOs.Productos;
using Productos.Models;
using Productos.Repositories;

namespace Productos.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var productos = await _productoRepository.ObtenerTodosAsync();
        return productos.Select(MapearADto).ToList();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id);
        return producto is null ? null : MapearADto(producto);
    }

    public async Task<(ProductoDto? Producto, string? Error)> CrearAsync(ProductoRequest request)
    {
        if (!await _productoRepository.TipoProductoExisteAsync(request.TipoProductoId))
        {
            return (null, "El tipo de producto indicado no existe.");
        }

        var producto = new Producto
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Precio = request.Precio,
            Stock = request.Stock,
            TipoProductoId = request.TipoProductoId
        };

        await _productoRepository.AgregarAsync(producto);
        await _productoRepository.GuardarCambiosAsync();

        var creado = await _productoRepository.ObtenerPorIdAsync(producto.Id);
        return (MapearADto(creado!), null);
    }

    public async Task<(ProductoDto? Producto, string? Error)> ActualizarAsync(int id, ProductoRequest request)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id);
        if (producto is null)
        {
            return (null, "Producto no encontrado.");
        }

        if (!await _productoRepository.TipoProductoExisteAsync(request.TipoProductoId))
        {
            return (null, "El tipo de producto indicado no existe.");
        }

        producto.Nombre = request.Nombre;
        producto.Descripcion = request.Descripcion;
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.TipoProductoId = request.TipoProductoId;

        _productoRepository.Actualizar(producto);
        await _productoRepository.GuardarCambiosAsync();

        var actualizado = await _productoRepository.ObtenerPorIdAsync(id);
        return (MapearADto(actualizado!), null);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id);
        if (producto is null)
        {
            return false;
        }

        _productoRepository.Eliminar(producto);
        await _productoRepository.GuardarCambiosAsync();
        return true;
    }

    public async Task<List<TipoProductoDto>> ObtenerTiposAsync()
    {
        var tipos = await _productoRepository.ObtenerTiposAsync();
        return tipos.Select(t => new TipoProductoDto { Id = t.Id, Nombre = t.Nombre }).ToList();
    }

    private static ProductoDto MapearADto(Producto producto) => new()
    {
        Id = producto.Id,
        Nombre = producto.Nombre,
        Descripcion = producto.Descripcion,
        Precio = producto.Precio,
        Stock = producto.Stock,
        TipoProductoId = producto.TipoProductoId,
        TipoProductoNombre = producto.TipoProducto?.Nombre ?? string.Empty
    };
}
