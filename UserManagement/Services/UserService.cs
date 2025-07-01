using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.DTOs;
using UserManagement.Entities;
using UserManagement.Interfaces;

namespace UserManagement.Services
{
    // Implementación del servicio de usuario
    // User service implementation
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO dto)
        {
            // Validar campos obligatorios
            // Validate required fields
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email is required.");

            if (!IsValidEmail(dto.Email))
                throw new Exception("Invalid email format.");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new Exception("Password must be at least 6 characters long.");

            if (string.IsNullOrWhiteSpace(dto.FullName) || dto.FullName.Length < 3)
                throw new Exception("Full name must be at least 3 characters long.");

            if (!new[] { "usuario", "admin" }.Contains(dto.Role.ToLower()))
                throw new Exception("Invalid role. Only 'usuario' or 'admin' are allowed.");

            // Verificar duplicado
            // Check for duplicate email
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already registered.");

            // Generar hash de contraseña
            // Generate password hash
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var entity = new UserEntity
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role.ToLower(),
                PasswordHash = passwordHash,
            };

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDTO(entity);
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            var entity = await _context.Users.FindAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var usuarios = await _context.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();

            return usuarios.Select(MapToDTO).ToList();
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserUpdateDTO dto)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null)
                return false;

            if (dto.FullName != null)
                entity.FullName = dto.FullName;
            if (dto.Phone != null)
                entity.Phone = dto.Phone;
            if (dto.Role != null)
                entity.Role = dto.Role;
            if (dto.IsActive.HasValue)
                entity.IsActive = dto.IsActive.Value;

            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // Conversión entidad → DTO
        // Convert entity to DTO
        private static UserDTO MapToDTO(UserEntity entity)
        {
            return new UserDTO
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
                Phone = entity.Phone,
                Role = entity.Role,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
            };
        }

        // Validar formato de email con expresión regular simple
        // Validate email format with basic regex
        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, UserChangePasswordDTO dto)
        {
            if (
                string.IsNullOrWhiteSpace(dto.CurrentPassword)
                || string.IsNullOrWhiteSpace(dto.NewPassword)
            )
                return false;

            if (dto.NewPassword.Length < 6)
                return false;

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            var isCurrentValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
            if (!isCurrentValid)
                return false;

            // Encriptar nueva contraseña y actualizar
            // Encrypt new password and update
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
