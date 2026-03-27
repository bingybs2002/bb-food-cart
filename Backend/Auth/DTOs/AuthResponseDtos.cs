using Backend.Models.Account;

namespace Backend.Auth.DTOs;

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
    UserAccount? Customer);

public sealed record OperationMessageResponse(string Message);
