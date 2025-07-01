using Microsoft.EntityFrameworkCore;
using UserManagement.Entities;

namespace UserManagement.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor para inyección de dependencias
        // Constructor for dependency injection
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Tabla de usuarios
        // Users table
        public DbSet<UserEntity> Users { get; set; }

        // Configuración adicional del modelo
        // Additional model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Usuario
            // Configuration of the Usuario entity
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);

                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });
        }
    }
}
