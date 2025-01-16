using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tdev702.Auth.Database;
using Tdev702.Contracts.Config;

namespace Tdev702.Auth.Services;

public interface ITokenService
{
    Task<AccessTokenResponse> GetAccessTokenAsync(User user);
    ClaimsPrincipal ValidateToken(string token, bool validateLifetime = true);
}

public class TokenService : ITokenService
{
    private readonly AuthConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly TokenValidationParameters _tokenValidationParams;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration configuration, UserManager<User> userManager, ILogger<TokenService> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration.GetSection("auth").Get<AuthConfiguration>() 
                         ?? throw new InvalidOperationException("Auth configuration not found");
        
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

    public async Task<AccessTokenResponse> GetAccessTokenAsync(User user)
    {
        try
        {
            _logger.LogInformation("Generating access token for user: {Email}", user.Email);
            var tokenExpiration = TimeSpan.FromMinutes(60);
            var accessToken = await GenerateToken(user, tokenExpiration);
            var refreshToken = await GenerateToken(user, TimeSpan.FromDays(1));
        
            return new AccessTokenResponse()
            { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken, 
                ExpiresIn = (int)tokenExpiration.TotalSeconds 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate tokens for user: {Email}", user.Email);
            throw;
        }
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
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName ?? string.Empty),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName ?? string.Empty),
            new("email_verified", user.EmailConfirmed.ToString().ToLower())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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