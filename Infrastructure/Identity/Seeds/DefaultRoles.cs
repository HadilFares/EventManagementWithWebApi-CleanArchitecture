using Infra.Data.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infra.Data.Identity.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Roles.Exhibitor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Roles.Organizer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Roles.Participant.ToString()));

        }

    }
}
