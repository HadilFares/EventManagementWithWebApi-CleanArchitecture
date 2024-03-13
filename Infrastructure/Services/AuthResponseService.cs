using Application.Dtos.Account;
using Application.Interfaces.Authentification;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
    public class AuthResponseService:IAuthResponse
    {
        private readonly UserManager<User> _userManager;
        private readonly EventlyDbContext _context;
        public AuthResponseService(UserManager<User> userManager, EventlyDbContext context) {
            _userManager = userManager;
            _context = context;
        }
        public async Task<AuthResponse> SignUpAsync(SignUp model, string origin)
        {
            var auth = new AuthResponse();

            // Validate the model
            if (model == null)
            {
                auth.Message = "Invalid sign-up data.";
                return auth;
            }

            // Create a new User instance
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            // Attempt to create the user in the database
            var result = await _userManager.CreateAsync(user, model.Password);
            
                if (result.Succeeded)
            {
                // Create a new Organizer instance
                var organizer = new Organizer
                {
                    Id = user.Id, // Assign the UserId to the Organizer
                    ChiffreAffaires = 0            // Add additional properties as needed
                };

                // Add the Organizer to the context and save changes
                _context.Organizers.Add(organizer);
                await _context.SaveChangesAsync();

                // Optionally, send a verification email
                // var verificationUri = await SendVerificationEmail(organizer, origin);
                // Handle sending verification email here

                auth.Message = "SignUp Succeeded";
            }
            else
            {
                // Handle sign-up errors
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                auth.Message = $"SignUp Failed: {errors}";
            }

            return auth;
        }



        /* public async  Task<AuthResponse> SignUpAsync(SignUp model, string orgin)
         {
             var auth = new AuthResponse();

             // Create a new Organizer instance
             var organizer = new Organizer
             {
                 UserName = model.Username,
                 Email = model.Email,

             };

             // Attempt to create the user in the database
             var result = await _userManager.CreateAsync(organizer,model.Password);

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
             else
             {
                 auth.Message = "SignUp Succeeded";
             }
             return auth;
         }*/
    }
}
