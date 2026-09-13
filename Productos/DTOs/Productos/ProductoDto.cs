namespace Productos.DTOs.Productos;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int TipoProductoId { get; set; }
    public string TipoProductoNombre { get; set; } = string.Empty;
}
