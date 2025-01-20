using System.Net.Http.Headers;
using Tdev702.Auth.Extensions;
using Tdev702.Contracts.Auth;
using Tdev702.Contracts.Auth.Response;
using Tdev702.Contracts.Config;

namespace Tdev702.Auth.Services;

public interface IOAuthService
{
    string GetRedirectUrl(HttpContext context);
    Task<GoogleTokenResponse> GetGoogleAccessTokenAsync(HttpContext context);
    Task<FacebookTokenResponse> GetFacebookAccessTokenAsync(HttpContext context);
    
    Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken);
    Task<FacebookUserInfo> GetFacebookUserInfoAsync(string accessToken);
}

public class OAuthService : IOAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthConfiguration _authConfiguration;
    private readonly ILogger<OAuthService> _logger;

    public OAuthService(
        IHttpClientFactory httpClientFactory, 
        AuthConfiguration authConfiguration, 
        ILogger<OAuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _authConfiguration = authConfiguration;
        _logger = logger;
    }

    public string GetRedirectUrl(HttpContext context)
    {
        try
        {
            var provider = context.GetPathFromHttpContext("provider");
            var redirectUri = $"{context.Request.Scheme}://{context.Request.Host}/api/auth/external-callback/{provider}";
            var loginUrls = new Dictionary<string, string>
            {
                ["Google"] = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                             $"client_id={_authConfiguration.GoogleClientId}&" +
                             $"response_type=code&" +
                             $"scope=openid%20email%20profile&" +
                             $"redirect_uri={redirectUri}",

                ["Facebook"] = $"https://www.facebook.com/v12.0/dialog/oauth?" +
                               $"client_id={_authConfiguration.FacebookAppId}&" +
                               $"redirect_uri={redirectUri}&" +
                               $"scope=email,public_profile"
            };

            if (!loginUrls.ContainsKey(provider))
            {
                throw new ArgumentException($"Provider {provider} not supported");
            }
            
            return loginUrls[provider];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating redirect URL");
            throw;
        }
    }

    public async Task<GoogleTokenResponse> GetGoogleAccessTokenAsync(HttpContext context)
    {
        try
        {
            var code = context.GetUriParameterFromHttpContext("code");
            _logger.LogInformation("Exchanging code for Google token");
            var tokenResponse = await ExchangeCodeForTokenAsync("Google", code, context);
            if (!tokenResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to exchange code for token");
                throw new Exception("Failed to exchange code for token");
            }
            return await tokenResponse.Content.ReadFromJsonAsync<GoogleTokenResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exchanging code for token");
            throw;
        }
    }

    public async Task<FacebookTokenResponse> GetFacebookAccessTokenAsync(HttpContext context)
    {
        try
        {
            var code = context.GetUriParameterFromHttpContext("code");
            _logger.LogInformation("Exchanging code for Facebook token");
            var tokenResponse = await ExchangeCodeForTokenAsync("Facebook", code, context);
            if (!tokenResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to exchange code for token");
                throw new Exception("Failed to exchange code for token");
            }
            return await tokenResponse.Content.ReadFromJsonAsync<FacebookTokenResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exchanging code for token");
            throw;
        }
    }

    public async Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken)
    {
        try
        {
            _logger.LogInformation("Getting user info from Google");
            var infosClient = _httpClientFactory.CreateClient("GoogleUserInfo");
            infosClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfoResponse = await infosClient.GetAsync("userinfo");
            if (!userInfoResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user info");
                throw new Exception("Failed to get user info");
            }
            return await userInfoResponse.Content.ReadFromJsonAsync<GoogleUserInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user info");
            throw;
        }
    }

    public async Task<FacebookUserInfo> GetFacebookUserInfoAsync(string accessToken)
    {
        try
        {
            _logger.LogInformation("Getting user info from Facebook");
            var infosClient = _httpClientFactory.CreateClient("FacebookUserInfo");
            infosClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfoResponse = await infosClient.GetAsync(
                $"me?fields=id,email,first_name,last_name&access_token={accessToken}");

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user info");
                throw new Exception("Failed to get user info");
            }
            return await userInfoResponse.Content.ReadFromJsonAsync<FacebookUserInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user info");
            throw;
        }
    }

    private async Task<HttpResponseMessage> ExchangeCodeForTokenAsync(string provider, string code, HttpContext context)
    {
        var tokenClient = _httpClientFactory.CreateClient(provider == "Google"? "GoogleToken" : "FacebookToken");
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = provider == "Google" ? _authConfiguration.GoogleClientId : _authConfiguration.FacebookAppId,
            ["client_secret"] = provider == "Google" ? _authConfiguration.GoogleClientSecret : _authConfiguration.FacebookAppSecret,
            ["redirect_uri"] = $"https://{context.Request.Host}/api/auth/external-callback/{provider}",
            ["grant_type"] = "authorization_code"
        });

        return await tokenClient.PostAsync(provider == "Google" ?"token" : "access_token", content);

    }
}