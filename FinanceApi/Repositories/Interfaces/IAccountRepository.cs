using FinanceApi.Models.Entities;

namespace FinanceApi.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByIdAsync(Guid accountId);
        Task<IReadOnlyList<Account>> GetAccountsByUserIdAsync(Guid userId);
        Task<Account?> GetAccountByNameAsync(string name, Guid userId);
        Task CreateAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(Account account);
    }
}
