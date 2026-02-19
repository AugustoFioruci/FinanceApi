using FinanceApi.Models.Entities;
using FinanceApi.Requests;

namespace FinanceApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IReadOnlyList<Account>> GetUserAccountsAsync(Guid userId);
        Task<Account?> GetByIdAsync(Guid accountId, Guid userId);
        Task<Account> CreateAccountAsync(AccountCreateRequest accountCreateRequest, Guid userId);
        Task<Account> UpdateAccountAsync(AccountUpdateRequest accountUpdateRequest, Guid userId);
        Task DeleteAccountAsync(Guid accountId, Guid userId);
        Task<Account> DepositAccountAsync(Guid accountId, decimal amount, Guid userId);
        Task<Account> WithdrawAccountAsync(Guid accountId, decimal amount, Guid userId);
    }
}
