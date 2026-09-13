using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productos.DTOs.Productos;
using Productos.Models;
using Productos.Services;

namespace Productos.Controllers;

[ApiController]
[Authorize]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> ObtenerTodos()
    {
        return Ok(await _productoService.ObtenerTodosAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    [Authorize(Roles = Rol.Admin)]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoRequest request)
    {
        var (producto, error) = await _productoService.CrearAsync(request);
        if (error is not null)
        {
            return BadRequest(new { mensaje = error });
        }

        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto!.Id }, producto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Rol.Admin)]
    public async Task<ActionResult<ProductoDto>> Actualizar(int id, ProductoRequest request)
    {
        var (producto, error) = await _productoService.ActualizarAsync(id, request);
        if (error is not null)
        {
            return producto is null && error == "Producto no encontrado."
                ? NotFound(new { mensaje = error })
                : BadRequest(new { mensaje = error });
        }

        return Ok(producto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Rol.Admin)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _productoService.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}
