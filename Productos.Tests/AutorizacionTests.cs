using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Productos.DTOs.Productos;

namespace Productos.Tests;

public class AutorizacionTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public AutorizacionTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ObtenerProductos_SinToken_DevuelveUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/productos");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CrearProducto_ComoUser_DevuelveForbidden()
    {
        var client = _factory.CreateClient();
        var token = await AuthHelper.ObtenerTokenAsync(client, "user@serfinsa.com", "User123!");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("/api/productos", new ProductoRequest
        {
            Nombre = "Producto no permitido",
            Precio = 10,
            Stock = 1,
            TipoProductoId = 1
        });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
