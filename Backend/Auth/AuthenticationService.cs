using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Auth.DTOs;
using Backend.Data;
using Backend.Models.Account;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Auth;

public interface IAuthenticationService
{
    Task<IResult> RegisterAsync(RegisterCustomerRequest request);
    Task<IResult> LoginAsync(LoginRequest request);
    Task<IResult> RefreshAsync(RefreshRequest request);
    Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
    Task<IResult> GetCurrentUserAsync(ClaimsPrincipal principal);
    Task<IResult> LogoutAsync(ClaimsPrincipal principal, LogoutRequest? request);
    Task<IResult> ChangePasswordAsync(ClaimsPrincipal principal, ChangePasswordRequest request);
}

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthTokenService _authTokenService;

    public AuthenticationService(AppDbContext dbContext, UserManager<IdentityUser> userManager, IAuthTokenService authTokenService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _authTokenService = authTokenService;
    }

    public async Task<IResult> RegisterAsync(RegisterCustomerRequest request)
    {
        var normalizedPhoneNumber = NormalizePhoneNumber(request.PhoneNumber);
        var phoneExists = await _userManager.Users.AnyAsync(user =>
            user.PhoneNumber == normalizedPhoneNumber || user.UserName == normalizedPhoneNumber);

        if (phoneExists)
        {
            return Results.BadRequest(new OperationMessageResponse("Phone number is already registered."));
        }

        var user = new IdentityUser
        {
            UserName = normalizedPhoneNumber,
            PhoneNumber = normalizedPhoneNumber,
            PhoneNumberConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return Results.BadRequest(roleResult.Errors);
        }

        var customer = new AccountUser
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
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            var response = await CreateAndStoreTokensAsync(user, customer);
            await _dbContext.SaveChangesAsync();
            return Results.Ok(response);
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            throw;
        }
    }

    public async Task<IResult> LoginAsync(LoginRequest request)
    {
        var normalizedPhoneNumber = NormalizePhoneNumber(request.PhoneNumber);
        var user = await _userManager.Users.SingleOrDefaultAsync(existingUser =>
            existingUser.PhoneNumber == normalizedPhoneNumber || existingUser.UserName == normalizedPhoneNumber);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.BadRequest(new OperationMessageResponse("Invalid phone number or password."));
        }

        var customer = await FindCustomerAsync(user.Id);
        var response = await CreateAndStoreTokensAsync(user, customer);
        await _dbContext.SaveChangesAsync();
        return Results.Ok(response);
    }

    public async Task<IResult> RefreshAsync(RefreshRequest request)
    {
        var existingRefreshToken = await _dbContext.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .SingleOrDefaultAsync(refreshToken => refreshToken.Token == request.RefreshToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive)
        {
            return Results.BadRequest(new OperationMessageResponse("Refresh token is invalid or expired."));
        }

        existingRefreshToken.RevokedAtUtc = DateTime.UtcNow;
        var customer = await FindCustomerAsync(existingRefreshToken.UserId);
        var response = await CreateAndStoreTokensAsync(existingRefreshToken.User, customer);
        await _dbContext.SaveChangesAsync();
        return Results.Ok(response);
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var normalizedNumber = NormalizePhoneNumber(request.PhoneNumber);
        var user = await _userManager.Users.SingleOrDefaultAsync(existingUser =>
            existingUser.PhoneNumber == normalizedNumber || existingUser.UserName == normalizedNumber);

        if (user is null)
        {
            return Results.Ok(new OperationMessageResponse("If the account exists, a reset token has been generated."));
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        return Results.Ok(new
        {
            message = "If the account exists, a reset token has been generated.",
            resetToken = encodedResetToken
        });
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var normalizedNumber = NormalizePhoneNumber(request.PhoneNumber);
        var user = await _userManager.Users.SingleOrDefaultAsync(existingUser =>
            existingUser.PhoneNumber == normalizedNumber || existingUser.UserName == normalizedNumber);

        if (user is null)
        {
            return Results.BadRequest(new OperationMessageResponse("Invalid reset request."));
        }

        string decodedResetToken;
        try
        {
            decodedResetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetToken));
        }
        catch (FormatException)
        {
            return Results.BadRequest(new OperationMessageResponse("Reset token is invalid."));
        }

        var result = await _userManager.ResetPasswordAsync(user, decodedResetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        await _userManager.UpdateSecurityStampAsync(user);
        return Results.Ok(new OperationMessageResponse("Password reset successfully."));
    }

    public async Task<IResult> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        var user = await ResolveCurrentUserAsync(principal);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var customer = await FindCustomerAsync(user.Id);
        var roles = await _userManager.GetRolesAsync(user);
        return Results.Ok(new CurrentUserResponse(user.Id, user.PhoneNumber, roles, customer));
    }

    public async Task<IResult> LogoutAsync(ClaimsPrincipal principal, LogoutRequest? request)
    {
        var user = await ResolveCurrentUserAsync(principal);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var refreshTokenValue = request?.RefreshToken;
        var refreshTokens = _dbContext.RefreshTokens.Where(refreshToken =>
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

        await _dbContext.SaveChangesAsync();
        return Results.Ok(new OperationMessageResponse("Logged out successfully."));
    }

    public async Task<IResult> ChangePasswordAsync(ClaimsPrincipal principal, ChangePasswordRequest request)
    {
        var user = await ResolveCurrentUserAsync(principal);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        await _userManager.UpdateSecurityStampAsync(user);
        return Results.Ok(new OperationMessageResponse("Password updated successfully."));
    }

    private async Task<AuthResponse> CreateAndStoreTokensAsync(IdentityUser user, AccountUser? customer)
    {
        var tokens = await _authTokenService.CreateTokenAsync(user);
        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            Token = tokens.RefreshToken,
            UserId = user.Id,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc
        });

        return new AuthResponse(user.Id, user.PhoneNumber, customer?.Id, tokens.AccessToken, tokens.AccessTokenExpireAtUtc, tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc);
    }

    private async Task<IdentityUser?> ResolveCurrentUserAsync(ClaimsPrincipal principal)
    {
        var userId = _userManager.GetUserId(principal)
            ?? principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return string.IsNullOrWhiteSpace(userId) ? null : await _userManager.FindByIdAsync(userId);
    }

    private Task<AccountUser?> FindCustomerAsync(string userId) =>
        _dbContext.Customers.SingleOrDefaultAsync(existingCustomer => existingCustomer.UserId == userId);

    private static string NormalizePhoneNumber(string phoneNumber) =>
        new(phoneNumber.Where(char.IsDigit).ToArray());
}
