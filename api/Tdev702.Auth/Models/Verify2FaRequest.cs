using System.Text.Json.Serialization;

namespace Tdev702.Auth.Models;

public class Verify2FaRequest
{
    [JsonPropertyName("verification_code" )]
    public required string VerificationCode { get; set; } 
}