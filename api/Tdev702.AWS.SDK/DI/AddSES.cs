using Amazon.SimpleEmail;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.AWS.SDK.SES;

namespace Tdev702.AWS.SDK.DI;

public static class AddSES
{
    public static IServiceCollection AddSEService(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>();
        services.AddScoped<IEmailService, AwsSesEmailService>();
        return services;
    }
}