using FinanceApi.Models.Entities;
using FinanceApi.Requests;

namespace FinanceApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserCreateRequest userCreateRequest);
        Task<User> UpdateUserAsync(Guid id, string? newEmail=null, string? newPassword=null);
        Task DeleteUserAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User?>> GetAllUsersAsync();
    }
}
