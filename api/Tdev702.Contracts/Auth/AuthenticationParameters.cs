using System.Security.Cryptography;
using System.Text;

namespace Tdev702.Contracts.Auth;

public class AuthenticationParameters
{
    protected AuthenticationParameters() { }

    public AuthenticationParameters(string identityProvider, string clientId, string clientSecret, string redirectUri, string scope)
    {
        IdentityProvider = identityProvider;
        ClientId = clientId;
        ClientSecret = clientSecret;
        RedirectUri = redirectUri;
        Scope = scope;
        State = GenerateSecureToken();
        ChallengeVerifier = GenerateSecureToken();
        Challenge = CreateCodeChallenge(ChallengeVerifier);
    }

    public string IdentityProvider { get; init; }
    public string ClientId { get; init; }
    public string ClientSecret { get; init; } 
    public string RedirectUri { get; init; }
    public string ResponseType { get; } = "code";
    public string Scope { get; init; }
    public string FlowType { get; } = "authorization_code";
    public string? AuthorizationCode { get; set; }
    public string State { get; init; }
    public string Challenge { get; init; }
    public string ChallengeVerifier { get; init; }
    
    private string GenerateSecureToken()
    {
        var bytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return Base64UrlEncode(bytes);
    }
    private string CreateCodeChallenge(string codeVerifier)
    {
        using (var sha256 = SHA256.Create())
        {
            var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            return Base64UrlEncode(challengeBytes);
        }
    }

    private string Base64UrlEncode(byte[] input)
    {
        var base64 = Convert.ToBase64String(input);
        return base64
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}

public enum AuthenticationFlowType { AuthorizationCode, Implicit }
