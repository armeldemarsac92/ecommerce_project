using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tdev702.Contracts.Config;

namespace Tdev702.Auth.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, AuthConfiguration authConfiguration)
    {
        services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.SigninKey!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = authConfiguration.GoogleClientId;
                options.ClientSecret = authConfiguration.GoogleClientSecret;

            })
            .AddFacebook(options =>
            {
                options.ClientId = authConfiguration.FacebookAppId;
                options.ClientSecret = authConfiguration.FacebookAppSecret;
            });
        
        return services;
    }
}