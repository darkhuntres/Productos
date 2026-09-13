using System.ComponentModel.DataAnnotations;

namespace Productos.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Rol { get; set; } = Productos.Models.Rol.User;
}
