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
using Tdev702.Contracts.Auth.Request;
using Tdev702.Contracts.Config;

namespace Tdev702.Api.SDK.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<AuthTokenProvider>();
        services.AddTransient<AuthHeaderHandler>();
        services.AddAuthServices();
        services.AddRefitClientWithAuth<IProductEndpoints>("https://api-staging.epitechproject.fr");
        return services;
    }
    
        private static IServiceCollection AddRefitClientWithAuth<T>(
        this IServiceCollection services,
        string baseUrl) where T : class
        {
        
        services.AddRefitClient<T>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrl))
            .AddHttpMessageHandler<AuthHeaderHandler>()
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
            });;

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
        private readonly IAuthEndpoints _authApi;
        private readonly ICodeStateService _codeStateService;
        private AccessTokenResponse? _cachedTokenResponse;
        private DateTime _tokenExpirationTime;

        public AuthTokenProvider(
            ILogger<AuthTokenProvider> logger, 
            IAuthEndpoints authApi, 
            ICodeStateService codeStateService)
        {
            _logger = logger;
            _authApi = authApi;
            _codeStateService = codeStateService;
        }

        public async Task<string> GetTokenAsync()
        {
            if (_cachedTokenResponse is not null)
            {
                _tokenExpirationTime = DateTime.UtcNow.AddSeconds(_cachedTokenResponse.ExpiresIn);

                if (DateTime.UtcNow >= _tokenExpirationTime)
                {
                    await RefreshTokenAsync();
                }
                else
                {
                    return _cachedTokenResponse.AccessToken;
                }
            };
            var sendTwoFaCodeRequest = new Get2FaCodeRequest() { Email = "armeldemarsac@gmail.com" };
            await _authApi.Send2FaCodeAsync(sendTwoFaCodeRequest);
            var code = await _codeStateService.GetLastGeneratedCode("armeldemarsac@gmail.com");
            var loginResponse = await _authApi.LoginAsync(new LoginUserRequest() { Email = "armeldemarsac@gmail.com", TwoFactorCode = code });
            _cachedTokenResponse = loginResponse.Content;
            return _cachedTokenResponse.AccessToken;
        }

        private async Task RefreshTokenAsync()
        {
            try
            {
                var session = await _authApi.RefreshTokenAsync(new RefreshTokenRequest() { RefreshToken = _cachedTokenResponse.RefreshToken });
                if (session == null || string.IsNullOrEmpty(session.Content.AccessToken))
                    throw new UnauthorizedAccessException("Failed to authenticate.");

                _cachedTokenResponse = session.Content;
                _tokenExpirationTime = DateTime.UtcNow.AddSeconds(_cachedTokenResponse.ExpiresIn);
                _logger.LogInformation("Token refreshed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to authenticate with Supabase");
                throw new UnauthorizedAccessException("Failed to authenticate with Supabase.", ex);
            }
        }
    }
}