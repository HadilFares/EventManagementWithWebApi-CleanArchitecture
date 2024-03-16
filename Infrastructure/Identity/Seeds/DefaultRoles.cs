using Infra.Data.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


           /* var adminRole = await roleManager.FindByNameAsync(Roles.Roles.Admin.ToString());
            await roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, Roles.Roles.Admin.ToString()));
            var OrganizerRole = await roleManager.FindByNameAsync(Roles.Roles.Organizer.ToString());
            await roleManager.AddClaimAsync(OrganizerRole, new Claim(ClaimTypes.Role, Roles.Roles.Organizer.ToString()));
            var ParticipantRole = await roleManager.FindByNameAsync(Roles.Roles.Participant.ToString());
            await roleManager.AddClaimAsync(ParticipantRole, new Claim(ClaimTypes.Role, Roles.Roles.Participant.ToString()));
            var ExhibitorRole = await roleManager.FindByNameAsync(Roles.Roles.Exhibitor.ToString());
            await roleManager.AddClaimAsync(ExhibitorRole, new Claim(ClaimTypes.Role, Roles.Roles.Exhibitor.ToString()));
 */       }

    }
}
