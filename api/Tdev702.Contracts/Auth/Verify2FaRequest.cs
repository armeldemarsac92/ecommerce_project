using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth;

public class Verify2FaRequest
{
    [JsonPropertyName("verification_code" )]
    public required string VerificationCode { get; set; } 
}