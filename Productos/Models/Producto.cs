using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productos.Models;

public class Producto
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    public int Stock { get; set; }

    [Required]
    public int TipoProductoId { get; set; }

    public TipoProducto? TipoProducto { get; set; }
}
