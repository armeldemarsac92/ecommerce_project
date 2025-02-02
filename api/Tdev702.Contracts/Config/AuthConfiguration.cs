namespace Tdev702.Contracts.Config;

public class AuthConfiguration
{
    public required string JwtIssuer { get; init; }
    public required string JwtAudience { get; init; }
    public required string PrivateKey { get; init; }
    public required string PublicKey { get; init; }
    public required string SourceEmail { get; init; }
    public required string SmtpUsername { get; init; }
    public required string SmtpPassword { get; init; }
    public required string GoogleClientId { get; init; }
    public required string GoogleClientSecret { get; init; }
    public required string FacebookAppId { get; init; }
    public required string FacebookAppSecret { get; init; }
}