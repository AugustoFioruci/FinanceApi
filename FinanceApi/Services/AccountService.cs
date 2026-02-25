using FinanceApi.Exceptions;
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
                throw new AccountNotFoundException("Account Not Found");
            }
            return account;
        }
        public async Task<Account> CreateAccountAsync(AccountCreateRequest accountCreateRequest, Guid userId)
        {

            var existingAccount = await _accountRepository.GetAccountByNameAsync(accountCreateRequest.Name, userId);
            if (existingAccount != null)
            {
                throw new DuplicateAccountNameException(accountCreateRequest.Name);
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
                throw new DuplicateAccountNameException(accountUpdateRequest.Name);
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
                throw new AccountNotFoundException();
            }
            await _accountRepository.DeleteAccountAsync(existingAccount);
        }
        public async Task<Account> DepositAccountAsync(Guid accountId, decimal amount, Guid userId)
        {
            var account = await GetByIdAsync(accountId, userId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account Not Found");
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
                throw new AccountNotFoundException("Account Not Found");
            }
            if (account.Balance < amount)
            {
                throw new InsufficientFundsException();
            }
            account.Balance -= amount;
            await _accountRepository.UpdateAccountAsync(account);
            return account;
        }
    }
}
