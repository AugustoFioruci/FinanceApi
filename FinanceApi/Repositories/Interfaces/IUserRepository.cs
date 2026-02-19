using FinanceApi.Models.Entities;

namespace FinanceApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User?>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
