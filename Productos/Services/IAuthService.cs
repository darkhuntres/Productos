using Productos.DTOs.Auth;

namespace Productos.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
