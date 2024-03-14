using Application.Dtos.Account;
using Application.Dtos.Email;
using Application.Interfaces.Authentification;
using Application.Interfaces.Email;
using Domain.Entities;
using Infra.Data.Identity.Roles;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Identity.Services
{
    public class AuthResponseService : IAuthResponse
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EventlyDbContext _context;
        private readonly IEmailService _emailSender;
        public AuthResponseService(IEmailService emailSender,UserManager<User> userManager, RoleManager<IdentityRole> roleManager, EventlyDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        //SignUp
        public async Task<AuthResponse> SignUpAsync(SignUp model, string orgin)
        {
            var auth = new AuthResponse();

            var userEmail = await _userManager.FindByEmailAsync(model.Email);
            var userName = await _userManager.FindByNameAsync(model.Username);

            //checking the Email and username
            if (userEmail is not null)
                return new AuthResponse { Message = "Email is Already used ! " };

            if (userName is not null)
                return new AuthResponse { Message = "Username is Already used ! " };

            //fill
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            var role = await _roleManager.RoleExistsAsync(model.Role);

            //check the user Id and role
            if (user == null || !role)
                return new AuthResponse { Message = "Invalid ID or Role" };
            //{  return "Invalid ID or Role";}

            var userId = await _userManager.FindByIdAsync(user.Id);

            await _userManager.AddToRoleAsync(userId, model.Role);

            //check result
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }

                return new AuthResponse { Message = errors };
            }
           

            await _emailSender.SendEmailAsync(new EmailRequest()
            {
                ToEmail = user.Email,
                Body = $"Welcome to Evently!,For added security, your account needs to be validated  by an admin before you can access all features of our platform Once your account is validated, we will send you a link to set up your profile Thank you for joining Evently!",
                Subject = "Registration"
            });
          //  auth.Message = "SignUp Succeeded";

            return auth;
        }

    }
}