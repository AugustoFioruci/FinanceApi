using FinanceApi.Models.Entities;
using BCrypt.Net;
using FinanceApi.Interfaces;
namespace FinanceApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateUserAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            await _userRepository.CreateUserAync(user);
            return user;
        }
        public async Task<User> UpdateUserAsync(Guid id, string? newEmail = null, string? newPassword = null)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
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
                throw new Exception("User not found.");
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
