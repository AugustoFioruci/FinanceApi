using FinanceApi.Models.Entities;
using BCrypt.Net;
using FinanceApi.Services.Interfaces;
using FinanceApi.Repositories.Interfaces;
using FinanceApi.Models.Enums;
using FinanceApi.Requests;
using FinanceApi.Exceptions;
namespace FinanceApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateUserAsync(UserCreateRequest userCreateRequest)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userCreateRequest.Email);
            if (existingUser != null)
            {
                throw new DuplicateEmailException();
            }
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = userCreateRequest.Name,
                Email = userCreateRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreateRequest.Password),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                isActive = true
            };
            await _userRepository.CreateUserAsync(user);
            return user;
        }
        public async Task<User> UpdateUserAsync(Guid id, string? newEmail = null, string? newPassword = null)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new AccountNotFoundException("User Not Found");
            }
            if (newEmail != null)
            {
                user.Email = newEmail;
            }
            if (newPassword != null)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }
            await _userRepository.UpdateUserAsync(user);
            return user;
        }
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new AccountNotFoundException("User Not Found");

            }
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<IEnumerable<User?>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
