using MassTransit;
using Tdev702.Auth.Consumer;
using Tdev702.Auth.Endpoints;
using Tdev702.Auth.SDK.Consumer;

namespace Tdev702.Auth.Extensions;

public static class MessagingExtensions  
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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

        if (env == "staging" || env == "dev")
        {
            services.AddMassTransit<I2FaCodeBus>(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.SetInMemorySagaRepositoryProvider();

                var consumerAssembly = typeof(TwoFactorCodeConsumer).Assembly;
                x.AddConsumers(consumerAssembly);
                x.AddSagaStateMachines(consumerAssembly);
                x.AddSagas(consumerAssembly);
                x.AddActivities(consumerAssembly);

                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(configuration["AWS:Region"], _ => {});
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
        return services;
    }
}

public interface I2FaCodeBus :
    IBus
{
}
