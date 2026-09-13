using System.Net.Http.Json;
using Productos.DTOs.Auth;

namespace Productos.Tests;

public static class AuthHelper
{
    public static async Task<string> ObtenerTokenAsync(HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Email = email,
            Password = password
        });

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return body!.Token;
    }
}
