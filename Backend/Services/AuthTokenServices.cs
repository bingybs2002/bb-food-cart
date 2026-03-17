using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    
    //holds the JWT configuration from appsetttings.json
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Key { get; set; } = default!;
        public int AccessTokenMinutes { get; set; } = 180;
        public int RefreshTokenDays { get; set; } = 30;
    }

    //Data Transfer Object that returns token to the client
    public sealed record AuthTokens(string AccessToken, 
                                    DateTime AccessTokenExpireAtUtc, 
                                    string RefreshToken, 
                                    DateTime RefreshTokenExpiresAtUtc);

    /* Wrapping this inside the interface so it could be 
      injected via dependency injection */
    public interface IAuthTokenService{ Task<AuthTokens> CreateTokenAsync(IdentityUser user);}

    //Actual Implementation of the token service
    public sealed class AuthTokenServices : IAuthTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<IdentityUser> _userManager;
        
        //constructor injection
        //jwtoptions -> configuration
        //usermanager -> access to user roles and identity system
        public AuthTokenServices(IOptions<JwtOptions> jwtoptions, 
                                 UserManager<IdentityUser> userManager)
        {
            _jwtOptions = jwtoptions.Value; 
            _userManager = userManager;
        }

        //Main function used by login endpoints, generate both access token and refresh tokens
        public async Task<AuthTokens> CreateTokenAsync(IdentityUser user)
        {
            var now = DateTime.UtcNow;

            //Calculate expiration times
            var accessTokenExpireAtUtc = now.AddMinutes(_jwtOptions.AccessTokenMinutes);
            var refreshTokenExpiresAtUtc = now.AddDays(_jwtOptions.RefreshTokenDays);
            
            //JWT access and refresh token. Refresh is encrypted with random
            var accessToken = await CreateAccessTokenAsync(user, accessTokenExpireAtUtc);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            //all token are returned to the caller
            return new AuthTokens(accessToken, accessTokenExpireAtUtc, refreshToken, refreshTokenExpiresAtUtc);
        }

        //create the signed JWT access token
        private async Task<string> CreateAccessTokenAsync(IdentityUser user, 
                                                          DateTime expiresAtUtc)
        {
            var roles = await _userManager.GetRolesAsync(user);
            //Create claims that will be sotred inside the JWT, claim represetns info about the user.
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName 
                                            ?? user.PhoneNumber ?? user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            //include phone number if present
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            }

            //add role claims for authorization
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //Create signing key from secret string
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            //Create signing credentials using Hmac sha 256
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //build the jwt token obj
            var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer,
                                            audience: _jwtOptions.Audience,
                                            claims: claims,
                                            notBefore: DateTime.UtcNow,
                                            expires: expiresAtUtc,
                                            signingCredentials: credentials);

            //convert the token object to a compact jwt string
            return new JwtSecurityTokenHandler().WriteToken(token);        }
    }

}
