using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class SeedRoles
    {
        public static async Task RolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Check and Create Admin Role
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            // Check and Create User Role
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // Check and Create Moderator Role
            if (!await roleManager.RoleExistsAsync("Moderator"))
                await roleManager.CreateAsync(new IdentityRole("Moderator"));
        }

    }
}