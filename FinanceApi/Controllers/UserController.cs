using FinanceApi.Models.Entities;
using FinanceApi.Requests;
using FinanceApi.Responses;
using FinanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FinanceApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateRequest request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var user = await _userService.UpdateUserAsync(id, request?.Email, request?.Password);
                return Ok(new UserResponse
                {
                    Email = user.Email,
                    Name = user.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(new UserResponse 
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(new UserResponse
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Name = user.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users.Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Name = u.Name
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
