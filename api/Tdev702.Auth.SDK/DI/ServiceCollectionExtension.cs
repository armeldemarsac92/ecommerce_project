using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;
using Tdev702.Auth.SDK.Consumer;
using Tdev702.Auth.SDK.Endpoints;
using Tdev702.Auth.SDK.Service;
using Tdev702.Contracts.Exceptions;

namespace Tdev702.Auth.SDK.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddSingleton<ICodeStateService, CodeStateService>();
        services.AddMassTransit(options =>
        {
            options.AddConsumer<TwoFactorCodeConsumer>(consumer =>
            {
                consumer.UseMessageRetry(r => 
                {
                    r.Interval(3, TimeSpan.FromSeconds(10));
        
                    r.Handle<EmptyCodeException>();
                });
            });
            
            options.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host("eu-central-1", h => 
                {
                });
        
                cfg.ConfigureEndpoints(context);
            });
            
        });
        
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