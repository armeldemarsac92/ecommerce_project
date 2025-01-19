using MassTransit;
using Tdev702.Auth.Consumer;
using Tdev702.Auth.Endpoints;

namespace Tdev702.Auth.Extensions;

public static class MessagingExtensions  
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateStripeCustomerConsumer>();
    
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
        
                cfg.UseMessageRetry(r => 
                {
                    r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                });
            });
        });
        return services;
    }
}