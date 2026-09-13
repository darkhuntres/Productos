using Productos.Models;

namespace Productos.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
}
