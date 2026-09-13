using Microsoft.AspNetCore.Mvc;
using Productos.DTOs.Auth;
using Productos.Services;

namespace Productos.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var resultado = await _authService.LoginAsync(request);
        if (resultado is null)
        {
            return Unauthorized(new { mensaje = "Email o contraseña incorrectos." });
        }

        return Ok(resultado);
    }
}
