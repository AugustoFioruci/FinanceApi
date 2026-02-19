using FinanceApi.Data;
using FinanceApi.Models.Entities;
using FinanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Repositories
{
    public class AccountRepository(AppDbContext context) : IAccountRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Account?> GetAccountByIdAsync(Guid userAccount)
        {
            return await _context.Accounts.FindAsync(userAccount);
        }
        public async Task<IReadOnlyList<Account>> GetAccountsByUserIdAsync(Guid userId)
        {
            return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }
        public async Task<Account?> GetAccountByNameAsync(string name, Guid userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Name == name && a.UserId == userId);
        }
        public async Task CreateAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAccountAsync(Account account)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
