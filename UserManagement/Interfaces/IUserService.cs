using UserManagement.DTOs;
using UserManagement.Entities;

namespace UserManagement.Interfaces
{
    // Interfaz para el servicio de usuario
    // Interface for user service
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(UserCreateDTO dto);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(Guid id, UserUpdateDTO dto);
        Task<UserDTO?> LoginAsync(UserLoginDTO dto);
        Task<bool> ChangePasswordAsync(Guid userId, UserChangePasswordDTO dto);
    }
}
