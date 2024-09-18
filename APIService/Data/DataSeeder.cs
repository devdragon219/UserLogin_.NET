using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace APIService.Data
{
    public static class DataSeeder
    {
        public static async Task SeedUsersAndRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // Ensure Admin role exists
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Failed to create Admin role: {string.Join(", ", roleResult.Errors)}");
                    }
                }

                // Create Admin User
                var adminUser = await userManager.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true
                    };

                    var userResult = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (!userResult.Succeeded)
                    {
                        throw new Exception($"Failed to create admin user: {string.Join(", ", userResult.Errors)}");
                    }

                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred during data seeding: " + ex.Message, ex);
            }
        }
    }
}
