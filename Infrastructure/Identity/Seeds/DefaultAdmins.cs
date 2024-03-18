using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Identity.Seeds
{
    public class DefaultAdmins
    {
    

        public static async Task SeedUsers(UserManager<User> userManager, EventlyDbContext _context)
        {
            #region defaultUser1
            var defaultUser1 = new User
            {
                UserName = "hedilfares30",
                Email = "hedilfares30@gmail.com",
                FirstName = "Fares",
                LastName = "Hadil",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            
            if (userManager.Users.All(u => u.Id != defaultUser1.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser1, "Evently12345");
                    await userManager.AddToRoleAsync(defaultUser1,"Admin");
                }
                // Create the default account
                var defaultAccount = new Account
                {
                    UserId = defaultUser1.Id, // Associate the account with the user
                    Status = AccountStatus.Active // Set the status of the account
                };

                // Add the default account to the DbSet<Account> in your EventlyDbContext
                 _context.Accounts.Add(defaultAccount);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            #endregion


           
            
        }
    }



}
