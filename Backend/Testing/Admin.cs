using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Testing
{
    public static class Admin
    {
        public static async Task SeedAdmin(IServiceProvider s)
        {
            await using var scope = s.CreateAsyncScope();

            var userManager = scope.ServiceProvider.
                GetRequiredService<UserManager<IdentityUser>>();
            var adminPhone = "9179"; 
            var adminPassword = "9179";
            var existAdmin = await userManager.Users.SingleOrDefaultAsync(x=>x.PhoneNumber==adminPhone || x.UserName == adminPhone);
            if(existAdmin != null)
            {

            }
            else
            {
                existAdmin = new IdentityUser
                {
                    UserName = adminPhone,
                    PhoneNumber = adminPassword
                };
                var result = await userManager.CreateAsync(existAdmin, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create the admin user!!!");
                }
                if (!await userManager.IsInRoleAsync(existAdmin, "Admin"))
                {
                    await userManager.AddToRoleAsync(existAdmin, "Admin");
                }
            }
        }
    }
}
