namespace UserManagement.DTOs
{
    // DTO para actualizar datos del usuario
    // DTO for updating user data
    public class UserUpdateDTO
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
}
