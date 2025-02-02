using Microsoft.AspNetCore.Identity;
using Tdev702.Auth.Services;
using Tdev702.Contracts.Database;

namespace Tdev702.Auth.Extensions;

public static partial class AuthServiceExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddHttpClient("googletoken", client =>
        {
            client.BaseAddress = new Uri("https://oauth2.googleapis.com/token");
        });

        services.AddHttpClient("GoogleUserInfo", client =>
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v2/");
        });

        services.AddHttpClient("facebooktoken", client =>
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v21.0/oauth/access_token");
        });

        services.AddHttpClient("FacebookUserInfo", client =>
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v21.0/");
        });
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IKeyService, KeyService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddTransient<IEmailSender<User>, EmailSender>();
        return services;
    }
}