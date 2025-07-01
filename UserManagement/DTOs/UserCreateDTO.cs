using System;

namespace UserManagement.DTOs
{
    // DTO para la creación de usuario
    // DTO for user creation
    public class UserCreateDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Role { get; set; } = "usuario";
        public string Password { get; set; } = null!;
    }
}
