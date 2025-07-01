using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

        public UsuariosController(IUserService userService)
        {
            _userService = userService;
        }

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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDTO dto)
        {
            var success = await _userService.UpdateUserAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpPut("{id:guid}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, UserChangePasswordDTO dto)
        {
            var success = await _userService.ChangePasswordAsync(id, dto);
            return success ? NoContent() : BadRequest("Password change failed.");
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            // Mostrar los claims en consola para depuración
            // Print all claims for debugging
            Console.WriteLine("\n[DEBUG] Claims recibidos:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"[CLAIM] {claim.Type} => {claim.Value}");
            }

            // Obtener el ID del usuario desde el claim estándar
            // Get user ID from standard claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Obtener el email y rol desde sus tipos estándar
            // Get email and role from standard claim types
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Devolver identidad autenticada
            // Return authenticated identity
            return Ok(
                new
                {
                    Id = userId,
                    Email = email,
                    Role = role,
                }
            );
        }
    }
}
