using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth;

public class FacebookTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}