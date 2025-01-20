using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Response;

public class FacebookTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}