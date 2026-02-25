using FinanceApi.Requests;
using FinanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request.Email, request.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var response = await _authService.RegisterAsync(new UserCreateRequest
                {
                    Email = registerRequest.Email,
                    Password = registerRequest.Password,
                    Name = registerRequest.UserName
                },
                new AccountCreateRequest
                {
                    Name = registerRequest.AccountName,
                    InicialBalance = registerRequest.InicialBalance
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
