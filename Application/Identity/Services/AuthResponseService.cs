using Application.Dtos.Accounts;
using Application.Dtos.Email;
using Application.Interfaces.Authentification;
using Application.Interfaces.Email;
using Application.Interfaces.Stripe;
using Domain.Entities;
using Domain.Settings;
using Infra.Data.Identity.Roles;

using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IStripeService _stripeService;
        private readonly JWT _Jwt;
        public JWT _JWTSettings { get; }
        public AuthResponseService(IOptions<JWT> JwtSettings, IEmailService emailSender,IStripeService stripeService, IOptions<JWT> jwt, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, EventlyDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _stripeService= stripeService;
            _Jwt = jwt.Value;
            _JWTSettings = JwtSettings.Value;
        }

        public async Task<ClaimsPrincipal> DecodeJwtTokenAsync(string token)
        {
            var key = Encoding.UTF8.GetBytes(_JWTSettings.Key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _JWTSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _JWTSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Adjust as needed
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                // Token validation failed
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    

    private async Task<JwtSecurityToken> CreateJwtAsync(User user)
        {
            var symmetricSecurityKey =
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSecurityToken = new JwtSecurityToken(
         issuer: _Jwt.Issuer,
         audience: _Jwt.Audience,
         claims: claims,
         expires: DateTime.Now.AddMinutes(120),
         signingCredentials: signingCredentials
        );

            return jwtSecurityToken;
        }

        //Generate RefreshToken
     /*   private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireOn = DateTime.UtcNow.AddDays(10),
                CreateOn = DateTime.UtcNow
            };
        }*/
      /*  public async Task<AuthResponse> RefreshTokenCheckAsync(string token)
        {
            var auth = new AuthResponse();

            //find the user that match the sent refresh token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                auth.Message = "Invalid Token";
                return auth;
            }

            // check if the refreshtoken is active
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                auth.Message = "Inactive Token";
                return auth;
            }



            //revoke the sent Refresh Tokens
            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtSecurityToken = await CreateJwtAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            auth.Email = user.Email;
            auth.Roles = roles.ToList();
            auth.ISAuthenticated = true;
            auth.UserName = user.UserName;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            auth.TokenExpiresOn = jwtSecurityToken.ValidTo;
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;

            return auth;
        }
      */
        

      /*  //revoke Refresh token
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            // check if the refreshtoken is active
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            //revoke the sent Refresh Tokens
            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();

            await _userManager.UpdateAsync(user);

            return true;
        }
      */
        //SignUp
        public async Task<AuthResponse> SignUpAsync(SignUp model, string origin)
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
                Status = AccountStatus.Pending,
                UserId = user.Id,
                AccountCreationDate = DateTime.UtcNow
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

        
          


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
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                return new AuthResponse { Message = "Role does not exist" };
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            // Send email verification
            await _emailSender.SendEmailAsync(new EmailRequest
                {
                    ToEmail = user.Email,
                    Body = $"Welcome to Evently!,For added security, your account needs to be validated  by an admin before you can access all features of our platform Once your account is validated, we will send you a link to set up your profile Thank you for joining Evently!",
                    Subject = "Registration"
                });
            auth.ISAuthenticated = true;
                auth.Email = user.Email;
                auth.UserName = user.UserName;
                auth.Message = "SignUp Succeeded";
                return auth;
            }
        


        public async Task<AuthResponse> LoginAsync(Login model)
        {
            var auth = new AuthResponse();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                auth.Message = "User not found";
                return auth;
            }

            var userpass = await _userManager.CheckPasswordAsync(user, model.Password);
            var userAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId== user.Id);
            if (userAccount?.Status != AccountStatus.Active)
            {
                auth.Message = "Your account is not active. Please contact support.";
                return auth;
            }
            //|| user.AccountId.Status != AccountStatus.Active

            if (!userpass)
            {
                auth.Message = "Email or Password is incorrect";
                return auth;
            }

            var roles = await _userManager.GetRolesAsync(user);


                var jwtSecurityToken = await CreateJwtAsync(user);

                auth.Id = user.Id;
                auth.Email = user.Email;
                auth.Roles = roles.ToList();
                auth.ISAuthenticated = true;
                auth.UserName = user.UserName;
                auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                auth.TokenExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime();
                auth.Message = "Login Succeeded ";


            //check if the user has any active refresh token
           /* if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                auth.RefreshToken = activeRefreshToken.Token;
                auth.RefreshTokenExpiration = activeRefreshToken.ExpireOn;
            }
            else
            //in case user has no active refresh token
            {
                var newRefreshToken = GenerateRefreshToken();
                auth.RefreshToken = newRefreshToken.Token;
                auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;

                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
            }
           */
            return auth;
            }
           


        }
    }
