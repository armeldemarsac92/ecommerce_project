using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tdev702.Auth.Services;
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
                var keyService = services.BuildServiceProvider().GetRequiredService<IKeyService>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = keyService.PublicKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = authConfiguration.JwtIssuer,
                    ValidAudience = authConfiguration.JwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        return services;
    }
}