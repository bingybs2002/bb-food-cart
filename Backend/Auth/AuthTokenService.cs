using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Auth.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Auth;

public interface IAuthTokenService
{
    Task<AuthTokens> CreateTokenAsync(IdentityUser user);
}

public sealed class JwtOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
    public int AccessTokenMinutes { get; set; } = 180;
    public int RefreshTokenDays { get; set; } = 30;
}

public sealed class AuthTokenService : IAuthTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthTokenService(IOptions<JwtOptions> jwtOptions, UserManager<IdentityUser> userManager)
    {
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager;
    }

    public async Task<AuthTokens> CreateTokenAsync(IdentityUser user)
    {
        var now = DateTime.UtcNow;
        var accessTokenExpireAtUtc = now.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var refreshTokenExpiresAtUtc = now.AddDays(_jwtOptions.RefreshTokenDays);
        var accessToken = await CreateAccessTokenAsync(user, accessTokenExpireAtUtc);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new AuthTokens(accessToken, accessTokenExpireAtUtc, refreshToken, refreshTokenExpiresAtUtc);
    }

    private async Task<string> CreateAccessTokenAsync(IdentityUser user, DateTime expiresAtUtc)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? user.PhoneNumber ?? user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
        }

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
