using Application.Dtos.Account;
using Application.Interfaces.Authentification;
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

namespace Infra.Data.Services
{
    public class AuthResponseService : IAuthResponse
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EventlyDbContext _context;
        public AuthResponseService(UserManager<User> userManager, EventlyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        /*  public async Task<AuthResponse> SignUpAsync(SignUp model, string origin)
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
              }
              else
              {
                  // Handle sign-up errors
                  var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                  auth.Message = $"SignUp Failed: {errors}";
              }

              return auth;
          }
        */




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
           
            await _userManager.AddToRoleAsync(user, Roles.Participant.ToString());

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

           

            return auth;
        }


        //Assign Roles
        public async Task<string> AssignRolesAsync(AssignRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var role = await _roleManager.RoleExistsAsync(model.Role);

            //check the user Id and role
            if (user == null || !role)
                return "Invalid ID or Role";

            //check if user is already assiged to selected role
            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            //check result
            if (!result.Succeeded)
                return "Something went wrong ";

            return string.Empty;
        }

    }
}