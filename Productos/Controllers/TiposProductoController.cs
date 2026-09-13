using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productos.DTOs.Productos;
using Productos.Services;

namespace Productos.Controllers;

[ApiController]
[Authorize]
[Route("api/tiposproducto")]
public class TiposProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public TiposProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TipoProductoDto>>> ObtenerTodos()
    {
        return Ok(await _productoService.ObtenerTiposAsync());
    }
}
