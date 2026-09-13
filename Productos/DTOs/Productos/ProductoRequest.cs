using System.ComponentModel.DataAnnotations;

namespace Productos.DTOs.Productos;

public class ProductoRequest
{
    [Required, MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    [Required]
    public int TipoProductoId { get; set; }
}
