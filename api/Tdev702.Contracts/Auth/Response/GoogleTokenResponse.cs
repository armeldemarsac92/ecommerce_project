using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Response;

public class GoogleTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
}

