using Application.Dtos.Account;
using Application.Dtos.Email;
using Application.Interfaces.AccountRepository;
using Application.Interfaces.Email;
using Application.Interfaces.UserRepository;
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
        private readonly IUserService _userService;
        public AccountService(EventlyDbContext context, UserManager<User> userManager, IEmailService emailSender,IUserService userService) : base(context)
        {
            _context = context;
            _emailSender = emailSender;
            _userService = userService;
        }

        public async Task<bool> EditProfile(UserDTO userUpdateDTO)
        {
            var user = await _userManager.FindByEmailAsync(userUpdateDTO.Email);

            if (user == null)
            {
                return false; 
            }

            // Update user information
            user.FirstName = userUpdateDTO.FirstName;
            user.LastName = userUpdateDTO.LastName;
            user.PhoneNumber = userUpdateDTO.Number;
            try
            {
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (DbUpdateException)
            {
                return false;
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
                if (account.User != null )
                {
                    // Send the email
                    var userRoles = await _userService.GetUserRolesAsync(account.User);
                    if (!userRoles.Contains("organizer"))
                    {
                        await _emailSender.SendEmailAsync(new EmailRequest
                        {
                            ToEmail = account.User.Email,
                            Body = $"Welcome to Evently! Your account has been successfully activated. You can now access all features of our platform. Thank you for joining Evently!",
                            Subject = "Account Activation"
                        });
                    }
                    else
                    {
                        await _emailSender.SendEmailAsync(new EmailRequest
                        {
                            ToEmail = account.User.Email,
                            Body = $"Welcome to Evently! Your account has been successfully activated. You can now access all features of our platform and do the process of subscription . Thank you for joining Evently!",
                            Subject = "Account Activation"
                        });
                    }
                }

                return true;
            }
            return false;

        }
    }




}