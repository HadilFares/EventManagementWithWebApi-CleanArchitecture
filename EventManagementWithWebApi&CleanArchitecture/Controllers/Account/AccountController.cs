using Application.Interfaces.AccountRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infra.Data.Identity.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(IAccountService accountService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
            _accountService = accountService;
            _roleManager = roleManager;
        }

      
        [HttpGet]
        [Route("GetPendingAccounts")]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingAccounts()
        {
            var pendingAccounts = await _accountService.GetPendingOrganizerAndParticipantAccountsAsync();
            return Ok(pendingAccounts);
        }

        [HttpGet]
        [Route("GetPendingExhibitorsAccounts")]
       // [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetPendingExhibitorsAccounts()
        {
            var pendingAccounts = await _accountService.GetPendingExhibitorAccountsAsync();
            return Ok(pendingAccounts);
        }

       [HttpPut("{id}/status/{status}")]
       [Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
       // [Authorize(Roles = "Admin")]
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
