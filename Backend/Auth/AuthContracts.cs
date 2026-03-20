using Backend.Models.Account;
using Backend.Models;

namespace Backend.Auth;

public sealed record RegisterCustomerRequest(
    string PhoneNumber,
    string Password,
    string Cosignee,
    Allergies Allergies,
    string StreetAddress,
    string City,
    string State,
    string ZipCode);

public sealed record LoginRequest(string PhoneNumber, string Password);
public sealed record RefreshRequest(string RefreshToken);
public sealed record ForgotPasswordRequest(string PhoneNumber);
public sealed record ResetPasswordRequest(string PhoneNumber, string ResetToken, string NewPassword);
public sealed record LogoutRequest(string? RefreshToken);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public sealed record AuthTokens(
    string AccessToken,
    DateTime AccessTokenExpireAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc);

public sealed record AuthResponse(
    string UserId,
    string? PhoneNumber,
    int? CustomerId,
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc);

public sealed record CurrentUserResponse(
    string UserId,
    string? PhoneNumber,
    IEnumerable<string> Roles,
    Customer? Customer);

public sealed record OperationMessageResponse(string Message);
