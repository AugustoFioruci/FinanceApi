using FinanceApi.Models.Entities;
namespace FinanceApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
