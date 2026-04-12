using Backend.Models;

namespace Backend.Auth.DTOs;

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
public sealed record UserRole(string PhoneNumber, string passwor);
