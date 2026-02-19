using FinanceApi.Repositories.Interfaces;
using FinanceApi.Responses;
using FinanceApi.Services.Interfaces;

namespace FinanceApi.Services
{
    public class AuthService(IUserService userService, ITokenService tokenService) : IAuthService
    {
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email)
            ?? throw new Exception("Invalid email or password.");

            var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!valid)
            {
                throw new Exception("Invalid email or password.");
            }
            return new AuthResponse
            {
                AccessToken = _tokenService.GenerateToken(user),
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };
        }
        public async Task<AuthResponse> RegisterAsync(string email, string password, string name)
        {
            var existingUser = await _userService.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("Email already in use.");
            }
            var user = new Models.Entities.User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            };
            await _userService.CreateUserAsync(email, password, name);
            return new AuthResponse
            {
                AccessToken = _tokenService.GenerateToken(user),
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };

        }
    }
}
