namespace UserManagement.DTOs
{
    // DTO para cambiar contraseña
    // DTO for changing password
    public class UserChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
