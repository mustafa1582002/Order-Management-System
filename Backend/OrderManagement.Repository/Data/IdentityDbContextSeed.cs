using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace OrderManagement.Repository.Data
{
    public static class IdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new IdentityUser()
                {
                    Email = "Mustafa1582002@gmail.com",
                    PhoneNumber = "201141927179",
                    UserName = "Mustafa1582002",
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
                await userManager.AddToRoleAsync(User, "Admin");
            }

        }
    }
}
