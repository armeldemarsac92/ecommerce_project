using MassTransit;
using Tdev702.Api.Consumer;

namespace Tdev702.Api.Extensions;

public static class MessagingExtensions  
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UpdateNutrimentsConsumer>();
            x.AddConsumer<CreateNutrimentsConsumer>();
    
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