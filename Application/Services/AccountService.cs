using Application.Dtos.Accounts;
using Application.Dtos.Email;
using Application.Interfaces.AccountRepository;
using Application.Interfaces.Email;
using Application.Interfaces.UserRepository;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

using Infrastructure.Context;
using Application.Interfaces.IBaseRepository;
namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly EventlyDbContext _context;
        private readonly IEmailService _emailSender;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IBaseRepository<Account> _baseRepository;

        public AccountService(EventlyDbContext context, UserManager<User> userManager, IEmailService emailSender, IUserService userService, IBaseRepository<Account> baseRepository)
        {
            _baseRepository = baseRepository;

            _context = context;
            _emailSender = emailSender;
            _userService = userService;
            _userManager = userManager;
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
            user.PhoneNumber = userUpdateDTO.PhoneNumber;
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

        public async Task<List<(Account account, string role)>> GetPendingOrganizerAndParticipantAccountsAsync()
        {

            var organizers = await _userManager.GetUsersInRoleAsync("Organizer");
            var participants = await _userManager.GetUsersInRoleAsync("Participant");

            // Get the IDs of the organizer and participant users
            var organizerUserIds = organizers.Select(u => u.Id).ToList();
            var participantUserIds = participants.Select(u => u.Id).ToList();

            var allPendingAccounts = new List<(Account account, string role)>();
            foreach (var organizer in organizers)
            {
                var organizerAccounts = await _context.Accounts
                    .Include(a => a.User)
                    .Where(a => a.UserId == organizer.Id && a.Status == AccountStatus.Pending)
                    .ToListAsync();

                allPendingAccounts.AddRange(organizerAccounts.Select(account => (account, "Organizer")));
            }

            foreach (var participant in participants)
            {
                var participantAccounts = await _context.Accounts
                    .Include(a => a.User)
                    .Where(a => a.UserId == participant.Id && a.Status == AccountStatus.Pending)
                    .ToListAsync();

                allPendingAccounts.AddRange(participantAccounts.Select(account => (account, "Participant")));
            }

            return allPendingAccounts;

        }


        public async Task<List<Account>> GetPendingExhibitorAccountsAsync()
        {

            var organizers = await _userManager.GetUsersInRoleAsync("Exhibitor");

            // Get the IDs of the organizer users
            var organizerUserIds = organizers.Select(u => u.Id).ToList();

            // Retrieve the accounts for the organizer users with a "Pending" status
            var pendingAccounts = await _context.Accounts
                .Include(a => a.User) // Include User navigation property to avoid additional queries
                .Where(a => organizerUserIds.Contains(a.UserId) && a.Status == AccountStatus.Pending)
                .ToListAsync();
            return pendingAccounts;
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

        public async Task<Account> GetAccount(Guid id)
        {
            return await _baseRepository.Get(id);
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _baseRepository.GetAll();

        }
    }




}