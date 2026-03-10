using Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace Backend.EndPoints.Admin
{
    public static class Admin
    {
        public static void AdminStatus(this IEndpointRouteBuilder r)
        {
            var app = r.MapGroup("Admin");

            app.MapPost("/PromoteToAdmin/{email}", async (string email, UserManager<IdentityUser> UserManager) =>
            {
                var user = await UserManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return Results.NotFound();

                }
                await UserManager.AddToRoleAsync(user, "Admin");

                return Results.Ok("User Promoted To Admin");
            }).RequireAuthorization();

            app.MapGet("/CheckAdminStatus/{email}", async (string email, UserManager<IdentityUser> userManager) =>
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                    return Results.NotFound();

                var roles = await userManager.GetRolesAsync(user);
                bool isAdmin = roles.Contains("Admin");
                return Results.Ok(isAdmin);
            });
        }
            
    }
}
