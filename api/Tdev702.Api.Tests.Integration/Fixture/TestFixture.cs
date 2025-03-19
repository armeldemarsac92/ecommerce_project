using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tdev702.Api.SDK.DI;
using Tdev702.AWS.SDK.SecretsManager;

namespace Tdev702.Api.Tests.Integration.Fixture;

public class TestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddEnvironmentVariables();
            });
        
        hostBuilder.AddAwsConfiguration(SecretType.Auth);

        hostBuilder.ConfigureServices((context, services) =>
        {
            // Add your API services
            services.AddApiServices();
        });
        
        var host = hostBuilder.Build();
        ServiceProvider = (ServiceProvider?)host.Services;
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}