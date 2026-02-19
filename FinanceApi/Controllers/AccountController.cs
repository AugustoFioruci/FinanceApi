using FinanceApi.Extensions;
using FinanceApi.Requests;
using FinanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FinanceApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpGet("me")]
        public async Task<IActionResult> GetUserAccounts()
        {
            var userId = User.GetUserId();
            var accounts = await _accountService.GetUserAccountsAsync(userId);
            return Ok(accounts);
        }
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetById(Guid accountId)
        {
            var userId = User.GetUserId();
            var account = await _accountService.GetByIdAsync(accountId, userId);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountCreateRequest accountCreateRequest)
        {
            var userId = User.GetUserId();
            var account = await _accountService.CreateAccountAsync(accountCreateRequest, userId);
            if (account == null)
            {
                return BadRequest("An account with the same name already exists.");
            }
            return CreatedAtAction(nameof(GetById), new { accountId = account.AccountId }, account);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAccount(AccountUpdateRequest accountUpdateRequest)
        {
            var userId = User.GetUserId();
            var updatedAccount = await _accountService.UpdateAccountAsync(accountUpdateRequest, userId);
            if (updatedAccount == null)
            {
                return NotFound();
            }
            return Ok(updatedAccount);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var userId = User.GetUserId();
            await _accountService.DeleteAccountAsync(accountId, userId);

            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> Deposit(Guid accountId, decimal newBalance)
        {
            var userId = User.GetUserId();
            var updatedAccount = await _accountService.DepositAccountAsync(accountId, newBalance, userId);
            if (updatedAccount == null)
            {
                return NotFound();
            }
            return Ok(updatedAccount);
        }
        [HttpPost]
        public async Task<IActionResult> Withdraw(Guid accountId, decimal newBalance)
        {
            var userId = User.GetUserId();
            var updatedAccount = await _accountService.WithdrawAccountAsync(accountId, newBalance, userId);
            if (updatedAccount == null)
            {
                return NotFound();
            }
            return Ok(updatedAccount);


        }
    }
}
