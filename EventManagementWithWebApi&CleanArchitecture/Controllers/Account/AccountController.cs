using Application.Interfaces.AccountRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }


        [HttpGet]
        [Route("GetPendingAccounts")]

        public async Task<IActionResult> GetPendingAccounts()
        {
            var pendingAccounts = await _accountService.GetPendingAccountsAsync();
            return Ok(pendingAccounts);
        }

        [HttpPut("{id}/status/{status}")]
        public async Task<IActionResult> UpdateAccountStatus(Guid id, AccountStatus status)
        {
            var result = await _accountService.UpdateAccountStatusAsync(id, status);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
        
        [HttpGet]
        [Route("GetAccount/{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            var account = await _accountService.Get(id);
            if (account != null)
            {
                return Ok(account);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccount()
        {
            var account = await _accountService.GetAll();
            if (account != null)
            {
                return Ok(account);
            }
            return NotFound();
        }


    }
}
