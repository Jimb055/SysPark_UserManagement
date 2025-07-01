using System;

namespace UserManagement.Entities
{
    // Entidad principal del usuario
    // Main user entity
    public class UserEntity
    {
        public Guid Id { get; set; }

        // Nombre completo del usuario
        // Full name of the user
        public string FullName { get; set; } = null!;

        // Correo electrónico
        // Email address
        public string Email { get; set; } = null!;

        // Teléfono (opcional)
        // Phone number (optional)
        public string? Phone { get; set; }

        // Rol del usuario (por defecto "usuario")
        // User role (default is "usuario")
        public string Role { get; set; } = "usuario";

        // Estado de activación
        // Activation status
        public bool IsActive { get; set; } = true;

        // Fecha de creación del usuario
        // User creation date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Fecha de última actualización (opcional)
        // Last update date (optional)
        public DateTime? UpdatedAt { get; set; }

        // Hash de la contraseña
        // Password hash
        public string PasswordHash { get; set; } = null!;
    }
}
