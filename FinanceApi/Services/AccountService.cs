using FinanceApi.Models.Entities;
using FinanceApi.Repositories.Interfaces;
using FinanceApi.Requests;
using FinanceApi.Services.Interfaces;

namespace FinanceApi.Services
{
    public class AccountService(IAccountRepository accountRepository) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<IReadOnlyList<Account>> GetUserAccountsAsync(Guid userId)
        {
            return await _accountRepository.GetAccountsByUserIdAsync(userId);
        }
        public async Task<Account?> GetByIdAsync(Guid accountId, Guid userId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null || account.UserId != userId)
            {
                return null;
            }
            return account;
        }
        public async Task<Account> CreateAccountAsync(AccountCreateRequest accountCreateRequest, Guid userId)
        {

            var existingAccount = await _accountRepository.GetAccountByNameAsync(accountCreateRequest.Name, userId);
            if (existingAccount != null)
            {
                return null;// Account with the same ID already exists for this user
            }
            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                Name = accountCreateRequest.Name,
                Balance = accountCreateRequest.InicialBalance,
                UserId = userId, 
                CreatedDate = DateTime.UtcNow
            };
            await _accountRepository.CreateAccountAsync(account);
            return account;
        }
        public async Task<Account> UpdateAccountAsync(AccountUpdateRequest accountUpdateRequest, Guid userId)
        {
            var existingAccount = await _accountRepository.GetAccountByNameAsync(accountUpdateRequest.Name, userId);
            if (existingAccount == null)
            {
                return null;// Account not found or does not belong to the user
            }
            existingAccount.Name = accountUpdateRequest.Name;
            await _accountRepository.UpdateAccountAsync(existingAccount);
            return existingAccount;
        }
        public async Task DeleteAccountAsync(Guid accountId, Guid userId)
        {
            var existingAccount = await GetByIdAsync(accountId, userId);
            if (existingAccount == null)
            {
                return;// Account not found or does not belong to the user
            }
            await _accountRepository.DeleteAccountAsync(existingAccount);
            return;
        }
        public async Task<Account> DepositAccountAsync(Guid accountId, decimal amount, Guid userId)
        {
            var account = await GetByIdAsync(accountId, userId);
            if (account == null)
            {
                return null;// Account not found or does not belong to the user
            }
            account.Balance += amount;
            await _accountRepository.UpdateAccountAsync(account);
            return account;
        }
        public async Task<Account> WithdrawAccountAsync(Guid accountId, decimal amount, Guid userId)
        {
            var account = await GetByIdAsync(accountId, userId);
            if (account == null)
            {
                return null;// Account not found or does not belong to the user
            }
            if (account.Balance < amount)
            {
                throw new Exception("Insufficient funds.");
            }
            account.Balance -= amount;
            await _accountRepository.UpdateAccountAsync(account);
            return account;
        }
    }
}
