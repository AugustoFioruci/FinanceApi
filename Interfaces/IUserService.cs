using FinanceApi.Models.Entities;

namespace FinanceApi.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(String email, String password);
        Task<User> UpdateUserAsync(Guid id, String? newEmail=null, string? newPassword=null);
        Task DeleteUserAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User?>> GetAllUsersAsync();
    }
}
