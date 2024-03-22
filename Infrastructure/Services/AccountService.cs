using Application.Dtos.Account;
using Application.Dtos.Email;
using Application.Interfaces.AccountRepository;
using Application.Interfaces.Email;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Data.Services
{
    public class AccountService : BaseRepository<Account>, IAccountService
    {
        private readonly EventlyDbContext _context;
        private readonly IEmailService _emailSender;
        private readonly UserManager<User> _userManager;
        public AccountService(EventlyDbContext context, UserManager<User> userManager, IEmailService emailSender) : base(context)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<bool> EditProfile(UserDTO userUpdateDTO)
        {
            var user = await _userManager.FindByEmailAsync(userUpdateDTO.Email);

            if (user == null)
            {
                return false; // User or associated account not found
            }

            // Update user information
            user.FirstName = userUpdateDTO.FirstName;
            user.LastName = userUpdateDTO.LastName;
            user.PhoneNumber = userUpdateDTO.Number;
            try
            {
                await _context.SaveChangesAsync();
                return true; // Update successful
            }
            catch (DbUpdateException)
            {
                return false; // Error occurred while saving changes
            }
        }


        public async Task<List<Account>> GetPendingAccountsAsync()
        {
            var pendingAccount = await FindByConditionAsync(account => account.Status == AccountStatus.Pending);
            return pendingAccount != null ? new List<Account> { pendingAccount } : new List<Account>();
        }

        public async Task<bool> UpdateAccountStatusAsync(Guid accountId, AccountStatus status)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account != null)
            {
                account.Status = status;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                // Check if the account has a user before sending the email
                if (account.User != null)
                {
                    // Send the email
                    await _emailSender.SendEmailAsync(new EmailRequest
                    {
                        ToEmail = account.User.Email,
                        Body = $"Welcome to Evently! Your account has been successfully activated. You can now access all features of our platform. Thank you for joining Evently!",
                        Subject = "Account Activation"
                    });
                }

                return true;
            }
            return false;

        }
    }




}