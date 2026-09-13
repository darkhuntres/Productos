using Microsoft.EntityFrameworkCore;
using Productos.Data;
using Productos.Models;

namespace Productos.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Usuario?> ObtenerPorEmailAsync(string email)
    {
        return _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }
}
