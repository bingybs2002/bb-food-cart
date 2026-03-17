using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

using Backend.Data;
using Backend.Models.Account;
using Backend.Models.Food;
using Backend.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Backend.EndPoints.Accounts;

public static class Register
{
    public static void RegisterAccounts(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/register", RegisterAsync);
        routes.MapPost("/login", LoginAsync);
        routes.MapPost("/refresh", RefreshAsync);
        routes.MapPost("/forgot-password", ForgotPasswordAsync);
        routes.MapPost("/reset-password", ResetPasswordAsync);
        routes.MapGet("/me", GetMeAsync).RequireAuthorization();
        routes.MapPost("/logout", LogoutAsync).RequireAuthorization();
        routes.MapPost("/change-password", ChangePasswordAsync).RequireAuthorization();
    }

    public sealed record RegisterCustomerRequest(
        string PhoneNumber,
        string Password,
        string Cosignee,
        Allergies Allergies,
        string StreetAddress,
        string City,
        string State,
        string ZipCode);

    public sealed record AuthResponse(
        string UserId,
        string? PhoneNumber,
        int? CustomerId,
        string AccessToken,
        DateTime AccessTokenExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc);

    public sealed record LoginRequest(string PhoneNumber, string Password);
    public sealed record RefreshRequest(string RefreshToken);
    public sealed record ForgetPasswordRequest(string PhoneNumber);
    public sealed record ResetPasswordRequest(string PhoneNumber, string ResetToken, string NewPassword);
    public sealed record LogoutRequest(string? RefreshToken);
    public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

    private static string NormalizedPhoneNumber(string phoneNumber) =>
        new(phoneNumber.Where(char.IsDigit).ToArray());

    private static async Task<IdentityUser?> GetCurrentUserAsync(
        ClaimsPrincipal principal,
        UserManager<IdentityUser> userManager)
    {
        var userId = userManager.GetUserId(principal)
            ?? principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return await userManager.FindByIdAsync(userId);
    }

    private static async Task<IResult> RegisterAsync(
        AuthDbContext dbContext,
        UserManager<IdentityUser> userManager,
        IAuthTokenService authTokenService,
        RegisterCustomerRequest request)
    {
        var normalizedPhoneNumber = NormalizedPhoneNumber(request.PhoneNumber);
        var phoneExists = await userManager.Users.AnyAsync(user =>
            user.PhoneNumber == normalizedPhoneNumber || user.UserName == normalizedPhoneNumber);

        if (phoneExists)
        {
            return Results.BadRequest(new { message = "Phone number is already registered." });
        }

        var user = new IdentityUser
        {
            UserName = normalizedPhoneNumber,
            PhoneNumber = normalizedPhoneNumber,
            PhoneNumberConfirmed = false
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Results.BadRequest(roleResult.Errors);
        }

        var customer = new Customer
        {
            UserId = user.Id,
            User = user,
            Cosignee = request.Cosignee,
            Allergies = request.Allergies,
            StreetAddress = request.StreetAddress,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode
        };

        try
        {
            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();

            var authResponse = await CreateAndStoreTokensAsync(dbContext, authTokenService, user, customer);
            await dbContext.SaveChangesAsync();

            return Results.Ok(authResponse);
        }
        catch
        {
            await userManager.DeleteAsync(user);
            throw;
        }
    }

    private static async Task<IResult> LoginAsync(
        AuthDbContext dbContext,
        UserManager<IdentityUser> userManager,
        IAuthTokenService authTokenService,
        LoginRequest request)
    {
        var normalizedPhoneNumber = NormalizedPhoneNumber(request.PhoneNumber);
        var user = await userManager.Users.SingleOrDefaultAsync(existingUser =>
            existingUser.PhoneNumber == normalizedPhoneNumber || existingUser.UserName == normalizedPhoneNumber);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.BadRequest(new { message = "Invalid phone number or password." });
        }

        var customer = await dbContext.Customers.SingleOrDefaultAsync(existingCustomer =>
            existingCustomer.UserId == user.Id);
        var authResponse = await CreateAndStoreTokensAsync(dbContext, authTokenService, user, customer);

        await dbContext.SaveChangesAsync();

        return Results.Ok(authResponse);
    }

    private static async Task<AuthResponse> CreateAndStoreTokensAsync(
        AuthDbContext dbContext,
        IAuthTokenService authTokenService,
        IdentityUser user,
        Customer? customer)
    {
        var tokens = await authTokenService.CreateTokenAsync(user);

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            Token = tokens.RefreshToken,
            UserId = user.Id,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc
        });

        return new AuthResponse(
            user.Id,
            user.PhoneNumber,
            customer?.Id,
            tokens.AccessToken,
            tokens.AccessTokenExpireAtUtc,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiresAtUtc);
    }

    private static async Task<IResult> RefreshAsync(
        AuthDbContext dbContext,
        IAuthTokenService authTokenService,
        RefreshRequest request)
    {
        var existingRefreshToken = await dbContext.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .SingleOrDefaultAsync(refreshToken => refreshToken.Token == request.RefreshToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive)
        {
            return Results.BadRequest(new { message = "Refresh token is invalid or expired." });
        }

        existingRefreshToken.RevokedAtUtc = DateTime.UtcNow;

        var customer = await dbContext.Customers.SingleOrDefaultAsync(existingCustomer =>
            existingCustomer.UserId == existingRefreshToken.UserId);
        var authResponse = await CreateAndStoreTokensAsync(
            dbContext,
            authTokenService,
            existingRefreshToken.User,
            customer);

        await dbContext.SaveChangesAsync();
        return Results.Ok(authResponse);
    }

    private static async Task<IResult> ForgotPasswordAsync(
        UserManager<IdentityUser> userManager,
        ForgetPasswordRequest request)
    {
        var normalizedNumber = NormalizedPhoneNumber(request.PhoneNumber);
        var user = await userManager.Users.SingleOrDefaultAsync(existingUser =>
            existingUser.PhoneNumber == normalizedNumber || existingUser.UserName == normalizedNumber);

        if (user is null)
        {
            return Results.Ok(new { message = "If the account exists, a reset token has been generated." });
        }

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        return Results.Ok(new
        {
            message = "If the account exists, a reset token has been generated.",
            resetToken = encodedResetToken
        });
    }

    private static async Task<IResult> ResetPasswordAsync(
        UserManager<IdentityUser> userManager,
        ResetPasswordRequest request)
    {
        var normalizedNumber = NormalizedPhoneNumber(request.PhoneNumber);
        var user = await userManager.Users.SingleOrDefaultAsync(existingUser =>
            existingUser.PhoneNumber == normalizedNumber || existingUser.UserName == normalizedNumber);

        if (user is null)
        {
            return Results.BadRequest(new { message = "Invalid reset request." });
        }

        string decodedResetToken;

        try
        {
            decodedResetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetToken));
        }
        catch (FormatException)
        {
            return Results.BadRequest(new { message = "Reset token is invalid." });
        }

        var result = await userManager.ResetPasswordAsync(user, decodedResetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        await userManager.UpdateSecurityStampAsync(user);

        return Results.Ok(new { message = "Password reset successfully." });
    }

    private static async Task<IResult> GetMeAsync(
        ClaimsPrincipal principal,
        AuthDbContext dbContext,
        UserManager<IdentityUser> userManager)
    {
        var user = await GetCurrentUserAsync(principal, userManager);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var customer = await dbContext.Customers.SingleOrDefaultAsync(existingCustomer =>
            existingCustomer.UserId == user.Id);
        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new
        {
            userId = user.Id,
            phoneNumber = user.PhoneNumber,
            roles,
            customer
        });
    }

    private static async Task<IResult> LogoutAsync(
        ClaimsPrincipal principal,
        AuthDbContext dbContext,
        UserManager<IdentityUser> userManager,
        LogoutRequest? request)
    {
        var user = await GetCurrentUserAsync(principal, userManager);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var refreshTokenValue = request?.RefreshToken;
        var refreshTokens = dbContext.RefreshTokens.Where(refreshToken =>
            refreshToken.UserId == user.Id &&
            refreshToken.RevokedAtUtc == null &&
            refreshToken.ExpiresAtUtc > DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(refreshTokenValue))
        {
            refreshTokens = refreshTokens.Where(refreshToken => refreshToken.Token == refreshTokenValue);
        }

        var tokensToRevoke = await refreshTokens.ToListAsync();

        foreach (var refreshToken in tokensToRevoke)
        {
            refreshToken.RevokedAtUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
        return Results.Ok(new { message = "Logged out successfully." });
    }

    private static async Task<IResult> ChangePasswordAsync(
        ClaimsPrincipal principal,
        UserManager<IdentityUser> userManager,
        ChangePasswordRequest request)
    {
        var user = await GetCurrentUserAsync(principal, userManager);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        await userManager.UpdateSecurityStampAsync(user);

        return Results.Ok(new { message = "Password updated successfully." });
    }
}
