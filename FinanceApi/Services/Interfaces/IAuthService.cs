using FinanceApi.Requests;
using FinanceApi.Responses;

namespace FinanceApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RegisterAsync(UserCreateRequest userRequest, AccountCreateRequest accountRequest);
    }
}
