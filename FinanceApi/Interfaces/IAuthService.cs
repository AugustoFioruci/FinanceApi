using FinanceApi.Responses;

namespace FinanceApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RegisterAsync(string email, string password);
    }
}
