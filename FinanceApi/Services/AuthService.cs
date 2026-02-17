
using FinanceApi.Interfaces;
using FinanceApi.Responses;

namespace FinanceApi.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email)
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
        public async Task<AuthResponse> RegisterAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
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
            await _userRepository.CreateUserAync(user);
            return new AuthResponse
            {
                AccessToken = _tokenService.GenerateToken(user),
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };

        }
    }
}
