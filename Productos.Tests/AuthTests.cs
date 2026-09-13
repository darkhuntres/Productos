using System.Net;
using System.Net.Http.Json;
using Productos.DTOs.Auth;

namespace Productos.Tests;

public class AuthTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public AuthTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ConCredencialesValidas_DevuelveToken()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Email = "admin@serfinsa.com",
            Password = "Admin123!"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.False(string.IsNullOrWhiteSpace(body?.Token));
        Assert.Equal("Admin", body!.Rol);
    }

    [Fact]
    public async Task Login_ConCredencialesInvalidas_DevuelveUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Email = "admin@serfinsa.com",
            Password = "contraseña-incorrecta"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
