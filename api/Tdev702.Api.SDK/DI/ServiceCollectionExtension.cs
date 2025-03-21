using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;
using Tdev702.Api.Endpoints;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Auth.SDK.DI;
using Tdev702.Auth.SDK.Endpoints;
using Tdev702.Auth.SDK.Service;
using Tdev702.Auth.Services;
using Tdev702.Contracts.Auth.Request;
using Tdev702.Contracts.Config;
using Tdev702.Contracts.Database;

namespace Tdev702.Api.SDK.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var apiUrl = "https://api-staging.epitechproject.fr";
        if (env == "dev") apiUrl = "https://localhost:7143"; 
        services.AddSingleton<AuthTokenProvider>();
        services.AddTransient<AuthHeaderHandler>();
        services.AddRefitClientWithAuth<IProductEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<ICategoryEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<IBrandEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<IInventoryEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<IOrderEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<ICustomerEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<IOpenFoodFactEndpoints>(apiUrl);
        services.AddRefitClientWithAuth<ITagEndpoints>(apiUrl);
        return services;
    }
    
        private static IServiceCollection AddRefitClientWithAuth<T>(
        this IServiceCollection services,
        string baseUrl) where T : class
        {

            services.AddRefitClient<T>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrl))
                .AddHttpMessageHandler<AuthHeaderHandler>();
            // .AddStandardResilienceHandler(options =>
            // {
            //     options.AttemptTimeout = new HttpTimeoutStrategyOptions()
            //     {
            //         Timeout = TimeSpan.FromSeconds(10)
            //     };
            //     options.TotalRequestTimeout = new HttpTimeoutStrategyOptions()
            //     {
            //         Timeout = TimeSpan.FromSeconds(60)
            //     };
            //     options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);
            //     options.Retry.MaxRetryAttempts = 3;
            //     options.Retry.BackoffType = DelayBackoffType.Exponential;
            // });;

        return services;
    }

    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly AuthTokenProvider _authTokenProvider;
        private readonly ILogger<AuthHeaderHandler> _logger;

        public AuthHeaderHandler(AuthTokenProvider authTokenProvider, ILogger<AuthHeaderHandler> logger)
        {
            _authTokenProvider = authTokenProvider;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("AuthHeaderHandler.SendAsync called");
            var token = await _authTokenProvider.GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogInformation("Added Authorization header");
            return await base.SendAsync(request, cancellationToken);
        }
    }

    public class AuthTokenProvider
    {
        private readonly ILogger<AuthTokenProvider> _logger;
        private readonly ITokenService _tokenService;
        private AccessTokenResponse? _cachedTokenResponse;

        public AuthTokenProvider(
            ILogger<AuthTokenProvider> logger,
            ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<string> GetTokenAsync()
        {
            if (_cachedTokenResponse != null) return _cachedTokenResponse.AccessToken;
            var testUser = new User(){Email = "armeldemarsac@gmail.com", UserName = "armeldemarsac@gmail.com", Id = "d1eb3b9f-e1a3-40f2-a7e4-3d73cb050605", EmailConfirmed = true};
            _cachedTokenResponse = await _tokenService.GetAccessTokenAsync(testUser);
            return _cachedTokenResponse.AccessToken;
        }
    }
}