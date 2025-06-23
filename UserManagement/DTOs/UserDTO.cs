using System;

namespace UserManagement.DTOs
{
    // DTO para exponer datos del usuario
    // DTO for exposing user data
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
