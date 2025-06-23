namespace UserManagement.DTOs
{
    // DTO para el inicio de sesión
    // DTO for user login
    public class UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
