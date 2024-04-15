using Application.Dtos.Account;
using Application.Dtos.Email;
using Application.Interfaces.Authentification;
using Application.Interfaces.Email;
using Application.Interfaces.IBaseRepository;
using Application.Interfaces.UserRepository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthResponse _authResponse;
        private readonly EventlyDbContext _eventlyDbContext;
        private readonly IEmailService _emailService;
        private readonly IAccount _account;
        public UserService(UserManager<User> userManager,IEmailService emailService,EventlyDbContext eventlyDbContext) {
            _userManager = userManager;
           _eventlyDbContext= eventlyDbContext;
            _emailService= emailService;


        }

        public async Task<AuthResponse> AddUser(SignUp model, string orgin)
        {
            
                var auth = new AuthResponse();

                // Check if email and username are already in use
                var userEmail = await _userManager.FindByEmailAsync(model.Email);
                var userName = await _userManager.FindByNameAsync(model.Username);

                if (userEmail != null)
                    return new AuthResponse { Message = "Email is already in use!" };

                if (userName != null)
                    return new AuthResponse { Message = "Username is already in use!" };

                // Create the user
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,

                };


                var result = await _userManager.CreateAsync(user, model.Password);
                var account = new Account
                {
                    //  Id = Guid.NewGuid(),
                    Status = AccountStatus.Active,
                    UserId = user.Id,
                    AccountCreationDate = DateTime.UtcNow
                };

            _eventlyDbContext.Accounts.Add(account);
                await _eventlyDbContext.SaveChangesAsync();

                // Check if user creation was successful
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description}, ";
                    }

                    return new AuthResponse { Message = errors };
                }


                // Assign the user to the role
                /*var roleExists = await _roleManager.RoleExistsAsync(model.Role);
                if (!roleExists)
                {
                    return new AuthResponse { Message = "Role does not exist" };
                }*/

                await _userManager.AddToRoleAsync(user, model.Role);

                // Send email verification
                await _emailService.SendEmailAsync(new EmailRequest
                {
                    ToEmail = user.Email,
                    Body = $"Welcome to Evently!, your account is validated, you can now use all our features , Thank you for joining Evently!",
                    Subject = "Registration"
                });
                auth.ISAuthenticated = true;
                auth.Email = user.Email;
                auth.UserName = user.UserName;
                auth.Message = "SignUp Succeeded";
                auth.Id= user.Id;
                return auth;
            }

        

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();

        }

    }
}
