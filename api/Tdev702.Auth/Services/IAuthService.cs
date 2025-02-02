using System.Net.Http.Headers;
using System.Text.Json;
using Tdev702.Auth.Models;
using Tdev702.Contracts.Auth.Response;
using Tdev702.Contracts.Config;

namespace Tdev702.Auth.Services;

public interface IAuthService
{
    public string BuildLoginUri(AuthenticationParameters parameters);
    public Task<TokenResponse> ExchangeCodeForTokens(AuthenticationParameters parameters);
    public Task<FacebookUserInfo> GetFacebookUserInfoAsync(string accessToken);
    public Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken);
}

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IHttpClientFactory httpClientFactory, 
        AuthConfiguration configuration, 
        ILogger<AuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public string BuildLoginUri(AuthenticationParameters parameters)
    {
        
        var queryParams = new Dictionary<string, string>
        {
            ["client_id"] = parameters.ClientId,
            ["response_type"] = parameters.ResponseType,
            ["scope"] = parameters.Scope,
            ["redirect_uri"] = parameters.RedirectUri,
            ["state"] = parameters.State,
            ["code_challenge"] = parameters.Challenge,
            ["code_challenge_method"] = "S256"
        };

        var baseUrl = parameters.IdentityProvider == "google" ? "https://accounts.google.com/o/oauth2/v2/auth" : "https://www.facebook.com/v12.0/dialog/oauth";
        var query = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
   
        return $"{baseUrl}?{query}";
    }

    public async Task<TokenResponse> ExchangeCodeForTokens(AuthenticationParameters parameters)
    {
        var tokenRequest = new Dictionary<string, string>
        {
            ["grant_type"] = parameters.FlowType,
            ["code"] = parameters.AuthorizationCode,
            ["redirect_uri"] = parameters.RedirectUri,
            ["client_id"] = parameters.ClientId,
            ["code_verifier"] = parameters.ChallengeVerifier,
        };
        
        if(parameters.IdentityProvider == "google") tokenRequest.Add("client_secret", parameters.ClientSecret);

        var clientName = $"{parameters.IdentityProvider}token".ToLower();

        var tokenClient = _httpClientFactory.CreateClient(clientName);
        var response = await tokenClient.PostAsync("", 
            new FormUrlEncodedContent(tokenRequest));

        if (!response.IsSuccessStatusCode)
            throw new Exception("Token exchange failed");

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TokenResponse>(content);
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
                $"me?fields=id,email,first_name,last_name");

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
}