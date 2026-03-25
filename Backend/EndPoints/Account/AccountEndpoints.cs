using System.Security.Claims;
using System.Text.Json;
using Backend.Auth;
using Backend.Auth.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.EndPoints.Account;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/register", (RegisterCustomerRequest request, 
                                     IAuthenticationService service) =>
                service.RegisterAsync(request));
        routes.MapPost("/login", (LoginRequest request, 
                                  IAuthenticationService service) =>
                service.LoginAsync(request));
        routes.MapPost("/refresh", (RefreshRequest request, 
                                    IAuthenticationService service) =>
                service.RefreshAsync(request));
        routes.MapPost("/forgot-password", (ForgotPasswordRequest request, 
                                            IAuthenticationService service) =>
                service.ForgotPasswordAsync(request));
        routes.MapPost("/reset-password", (ResetPasswordRequest request, 
                                            IAuthenticationService service) =>
                service.ResetPasswordAsync(request));
        routes.MapGet("/me", (ClaimsPrincipal principal, 
                                IAuthenticationService service) =>
                service.GetCurrentUserAsync(principal)).RequireAuthorization();
        routes.MapPost("/logout", async (ClaimsPrincipal principal, 
                                        HttpRequest request, 
                                        IAuthenticationService service) =>
        {
            LogoutRequest? logoutRequest = null;

            if (request.ContentLength is > 0)
            {
                logoutRequest = await request.ReadFromJsonAsync<LogoutRequest>
                    (new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }

            return await service.LogoutAsync(principal, logoutRequest);
        }).RequireAuthorization();
        routes.MapPost("/change-password", (ClaimsPrincipal principal, 
                                            ChangePasswordRequest request, 
                                            IAuthenticationService service) =>
            service.ChangePasswordAsync(principal, request)).RequireAuthorization();
        routes.MapPost("/is-admin",
                        async(UserRole request,
                        UserManager<IdentityUser> userManager) =>
            {
                var NPhoneNumber = NormalizePhoneNumber(request.PhoneNumber);
                var user = await userManager.Users.SingleOrDefaultAsync(x => 
                x.PhoneNumber == NPhoneNumber || x.UserName == NPhoneNumber);
                if (user is null) return Results.NotFound();
                var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                return Results.Ok(isAdmin);
            });
        routes.MapPost("/promote-to-admin",
                        async (UserRole request,
                        UserManager<IdentityUser> userManager) =>
            {
                var NPhoneNumber = NormalizePhoneNumber(request.PhoneNumber);
                var user = await userManager.Users.SingleOrDefaultAsync(x =>
                    x.PhoneNumber == NPhoneNumber || x.UserName == NPhoneNumber);
                if (user is null) return Results.NotFound("Invalid User!");
                var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin is true)return Results.Ok("Already admin");
                await userManager.AddToRoleAsync(user, "Admin");
                return Results.Ok("Successfully added admin!");
            }).RequireAuthorization("Admin"); 
        return routes;
    }
    private static string NormalizePhoneNumber(string phoneNumber) =>
            new(phoneNumber.Where(char.IsDigit).ToArray());
}
