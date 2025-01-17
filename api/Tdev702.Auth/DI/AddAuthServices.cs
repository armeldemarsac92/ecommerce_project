using Microsoft.AspNetCore.Identity;
using Tdev702.Auth.Database;
using Tdev702.Auth.Services;

namespace Tdev702.Auth.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddHttpClient("GoogleToken", client =>
        {
            client.BaseAddress = new Uri("https://oauth2.googleapis.com/");
        });

        services.AddHttpClient("GoogleUserInfo", client =>
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v2/");
        });

        services.AddHttpClient("FacebookToken", client =>
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v21.0/oauth/");
        });

        services.AddHttpClient("FacebookUserInfo", client =>
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v21.0/");
        });
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOAuthService, OAuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddTransient<IEmailSender<User>, EmailSender>();
        return services;
    }
}