using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tdev702.Auth.Database;
using Tdev702.Contracts.Config;

namespace Tdev702.Auth.Services;

public interface ITokenService
{
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    ClaimsPrincipal ValidateToken(string token, bool validateLifetime = true);
}

public class TokenService : ITokenService
{
    private readonly AuthConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly TokenValidationParameters _tokenValidationParams;

    public TokenService(IConfiguration configuration, UserManager<User> userManager)
    {
        _userManager = userManager;
        _configuration = configuration.GetSection("auth").Get<AuthConfiguration>()?? throw new InvalidOperationException("Auth configuration not found");;
        
        var key = Encoding.ASCII.GetBytes(_configuration.SigninKey);
        _tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuration.Issuer,
            ValidAudience = _configuration.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        return await GenerateToken(user, TimeSpan.FromMinutes(15));
    }

    public async Task<string> GenerateRefreshToken(User user)
    {
        return await GenerateToken(user, TimeSpan.FromDays(7));
    }

    public ClaimsPrincipal ValidateToken(string token, bool validateLifetime = true)
    {
        _tokenValidationParams.ValidateLifetime = validateLifetime;
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, _tokenValidationParams, out _);
        return principal;
    }

    private async Task<string> GenerateToken(User user, TimeSpan expiration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.SigninKey);
        var role = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, role[0]),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(expiration),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        );

        return tokenHandler.WriteToken(token);
    }
}