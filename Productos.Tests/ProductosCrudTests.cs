using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Productos.DTOs.Productos;

namespace Productos.Tests;

public class ProductosCrudTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public ProductosCrudTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<HttpClient> CrearClienteAdminAsync()
    {
        var client = _factory.CreateClient();
        var token = await AuthHelper.ObtenerTokenAsync(client, "admin@serfinsa.com", "Admin123!");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    [Fact]
    public async Task ListarProductos_ComoAdmin_DevuelveOk()
    {
        var client = await CrearClienteAdminAsync();

        var response = await client.GetAsync("/api/productos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var productos = await response.Content.ReadFromJsonAsync<List<ProductoDto>>();
        Assert.NotNull(productos);
    }

    [Fact]
    public async Task CrearProducto_ComoAdmin_DevuelveCreado()
    {
        var client = await CrearClienteAdminAsync();

        var response = await client.PostAsJsonAsync("/api/productos", new ProductoRequest
        {
            Nombre = "Teclado mecánico",
            Descripcion = "Switches rojos",
            Precio = 45.99m,
            Stock = 20,
            TipoProductoId = 1
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var producto = await response.Content.ReadFromJsonAsync<ProductoDto>();
        Assert.NotNull(producto);
        Assert.Equal("Teclado mecánico", producto!.Nombre);
    }

    [Fact]
    public async Task ActualizarProducto_ComoAdmin_DevuelveOk()
    {
        var client = await CrearClienteAdminAsync();

        var creado = await client.PostAsJsonAsync("/api/productos", new ProductoRequest
        {
            Nombre = "Mouse",
            Descripcion = "Inalámbrico",
            Precio = 15,
            Stock = 30,
            TipoProductoId = 1
        });
        var productoCreado = await creado.Content.ReadFromJsonAsync<ProductoDto>();

        var response = await client.PutAsJsonAsync($"/api/productos/{productoCreado!.Id}", new ProductoRequest
        {
            Nombre = "Mouse inalámbrico",
            Descripcion = "Inalámbrico con batería recargable",
            Precio = 18,
            Stock = 25,
            TipoProductoId = 1
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var actualizado = await response.Content.ReadFromJsonAsync<ProductoDto>();
        Assert.Equal("Mouse inalámbrico", actualizado!.Nombre);
        Assert.Equal(18, actualizado.Precio);
    }

    [Fact]
    public async Task EliminarProducto_ComoAdmin_DevuelveNoContent()
    {
        var client = await CrearClienteAdminAsync();

        var creado = await client.PostAsJsonAsync("/api/productos", new ProductoRequest
        {
            Nombre = "Monitor",
            Descripcion = "24 pulgadas",
            Precio = 150,
            Stock = 5,
            TipoProductoId = 1
        });
        var productoCreado = await creado.Content.ReadFromJsonAsync<ProductoDto>();

        var response = await client.DeleteAsync($"/api/productos/{productoCreado!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var consulta = await client.GetAsync($"/api/productos/{productoCreado.Id}");
        Assert.Equal(HttpStatusCode.NotFound, consulta.StatusCode);
    }
}
