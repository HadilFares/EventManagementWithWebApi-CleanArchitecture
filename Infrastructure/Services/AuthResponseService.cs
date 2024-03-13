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
            if (model == null)
            {
                auth.Message = "Invalid sign-up data.";
                return auth;
            }
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var usertest = await _userManager.FindByNameAsync(user.UserName);
                var organizer = new Organizer
                {
                  // Id = usertest.Id,
                ChiffreAffaires = 1000            
                };

                try
                {
                
                   /* var existingUser = await _context.Users.FindAsync(user.Id);
                    if (existingUser != null)
                    {
                        _context.Entry(existingUser).State = EntityState.Detached;
                    }*/

                    _context.Organizers.Add(organizer);
                    await _context.SaveChangesAsync();
                 



                    auth.Message = "SignUp Succeeded";
                }
                catch (Exception ex)
                {
                  
                    auth.Message = $"Error occurred during sign-up: {ex.Message}";
                }
            }
            else
            {
                // Handle sign-up errors
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                auth.Message = $"SignUp Failed: {errors}";
            }

            return auth;
        }




    }
}
