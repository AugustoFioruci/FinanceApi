using FinanceApi.Models.Entities;
namespace FinanceApi.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
