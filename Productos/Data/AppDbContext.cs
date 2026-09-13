using Microsoft.EntityFrameworkCore;
using Productos.Models;

namespace Productos.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<TipoProducto> TiposProducto => Set<TipoProducto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<TipoProducto>()
            .HasIndex(t => t.Nombre)
            .IsUnique();

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.TipoProducto)
            .WithMany(t => t.Productos)
            .HasForeignKey(p => p.TipoProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TipoProducto>().HasData(
            new TipoProducto { Id = 1, Nombre = "Electrónica" },
            new TipoProducto { Id = 2, Nombre = "Hogar" },
            new TipoProducto { Id = 3, Nombre = "Oficina" }
        );

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Email = "admin@serfinsa.com",
                // Password: Admin123!
                PasswordHash = "$2a$11$OmBcs9.cT6hz7PpqAiXpcuFPYQwWHmdOH.nQMrEtZuXvUPJr6xbtO",
                Rol = Rol.Admin
            },
            new Usuario
            {
                Id = 2,
                Email = "user@serfinsa.com",
                // Password: User123!
                PasswordHash = "$2a$11$YyMqjEZzYKEMqf3C6zc.7uxXDs6OEgEZM/eNizxILR2zkWKxcYOZq",
                Rol = Rol.User
            }
        );
    }
}
