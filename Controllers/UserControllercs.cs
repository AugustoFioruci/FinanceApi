using Microsoft.AspNetCore.Mvc;
using FinanceApi.Requests;
using FinanceApi.Models.Entities;
using FinanceApi.Interfaces;
namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserControllercs : ControllerBase
    {
        private readonly IUserService _userService;
        public UserControllercs(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
        {
            try
            {
                var user = await _userService.CreateUserAsync(request.Email, request.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
