using System.ComponentModel.DataAnnotations;

namespace Productos.Models;

public class TipoProducto
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
