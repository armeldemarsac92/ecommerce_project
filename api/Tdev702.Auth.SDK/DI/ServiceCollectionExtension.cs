using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;
using Tdev702.Auth.SDK.Endpoints;
using Tdev702.Auth.SDK.Service;
using Tdev702.Auth.Services;

namespace Tdev702.Auth.SDK.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var authUrl = "https://auth-staging.epitechproject.fr";
        if (env == "dev") authUrl = "https://localhost:7073"; 
        services.AddSingleton<IKeyService, KeyService>();
        services.AddSingleton<ITokenService, TestTokenService>();
        services.AddRefitClient<IAuthEndpoints>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://auth-staging.epitechproject.fr"))
            .AddStandardResilienceHandler(options =>
            {
                options.AttemptTimeout = new HttpTimeoutStrategyOptions()
                {
                    Timeout = TimeSpan.FromSeconds(10)
                };
                options.TotalRequestTimeout = new HttpTimeoutStrategyOptions()
                {
                    Timeout = TimeSpan.FromSeconds(60)
                };
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.BackoffType = DelayBackoffType.Exponential;
            });
        return services;
    }
}