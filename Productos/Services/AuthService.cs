using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Productos.DTOs.Auth;
using Productos.Repositories;

namespace Productos.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUsuarioRepository usuarioRepository, IOptions<JwtSettings> jwtOptions)
    {
        _usuarioRepository = usuarioRepository;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(request.Email);
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
        {
            return null;
        }

        var expiraEn = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiraMinutos);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiraEn,
            signingCredentials: credentials);

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = usuario.Email,
            Rol = usuario.Rol,
            ExpiraEn = expiraEn
        };
    }
}
