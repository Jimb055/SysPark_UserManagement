using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Interfaces;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService _userService;

        // Inyección del servicio de usuario
        // Inject user service
        public UsuariosController(IUserService userService)
        {
            _userService = userService;
        }

        // Crear nuevo usuario
        // Create new user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Obtener usuario por ID
        // Get user by ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        // Listar todos los usuarios
        // List all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        // Actualizar usuario
        // Update user
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDTO dto)
        {
            var success = await _userService.UpdateUserAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // Login de usuario (temporal en este microservicio)
        // User login (temporary in this microservice)
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDTO dto)
        {
            var result = await _userService.LoginAsync(dto);
            return result == null ? Unauthorized("Invalid credentials.") : Ok(result);
        }

        // Cambiar contraseña del usuario
        // Change user's password
        [HttpPut("{id:guid}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, UserChangePasswordDTO dto)
        {
            var success = await _userService.ChangePasswordAsync(id, dto);
            return success ? NoContent() : BadRequest("Password change failed.");
        }
    }
}
