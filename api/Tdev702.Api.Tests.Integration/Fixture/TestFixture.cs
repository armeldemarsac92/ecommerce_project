using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.DI;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Auth.SDK.Service;

namespace Tdev702.Api.Tests.Integration.Fixture;

public class TestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        var services = new ServiceCollection();

        services.AddApiServices();
        
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}