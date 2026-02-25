using FinanceApi.Exceptions;
using FinanceApi.Repositories.Interfaces;
using FinanceApi.Requests;
using FinanceApi.Responses;
using FinanceApi.Services.Interfaces;

namespace FinanceApi.Services
{
    public class AuthService(IUserService userService, ITokenService tokenService, IAccountService accountService) : IAuthService
    {
        private readonly IUserService _userService = userService;
        private readonly IAccountService _accountService = accountService;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email)
            ?? throw new InvalidCredentialsException();

            var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!valid)
            {
                throw new InvalidCredentialsException();
            }
            return new AuthResponse
            {
                AccessToken = _tokenService.GenerateToken(user),
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };
        }
        public async Task<AuthResponse> RegisterAsync(UserCreateRequest userRequest, AccountCreateRequest accountRequest)
        {
            var existingUser = await _userService.GetUserByEmailAsync(userRequest.Email);
            if (existingUser != null)
            {
                throw new InvalidCredentialsException();
            }
            
            var user = await _userService.CreateUserAsync(userRequest);
            var account = await _accountService.CreateAccountAsync(
                new AccountCreateRequest 
                {
                    Name = "Principal",
                    InicialBalance = accountRequest.InicialBalance
                }, user.UserId);

            return new AuthResponse
            {
                AccessToken = _tokenService.GenerateToken(user),
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };

        }
    }
}
