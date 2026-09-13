using Microsoft.EntityFrameworkCore;
using Productos.Data;
using Productos.Models;

namespace Productos.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Producto>> ObtenerTodosAsync()
    {
        return _context.Productos.Include(p => p.TipoProducto).ToListAsync();
    }

    public Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return _context.Productos.Include(p => p.TipoProducto).FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<bool> TipoProductoExisteAsync(int tipoProductoId)
    {
        return _context.TiposProducto.AnyAsync(t => t.Id == tipoProductoId);
    }

    public async Task AgregarAsync(Producto producto)
    {
        await _context.Productos.AddAsync(producto);
    }

    public void Actualizar(Producto producto)
    {
        _context.Productos.Update(producto);
    }

    public void Eliminar(Producto producto)
    {
        _context.Productos.Remove(producto);
    }

    public async Task<bool> GuardarCambiosAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public Task<List<TipoProducto>> ObtenerTiposAsync()
    {
        return _context.TiposProducto.AsNoTracking().ToListAsync();
    }
}
