using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Request;

public class Get2FaCodeRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
}