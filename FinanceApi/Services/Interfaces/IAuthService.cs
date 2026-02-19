using FinanceApi.Responses;

namespace FinanceApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RegisterAsync(string email, string password, string name);
    }
}
