namespace Tdev702.Contracts.Config;

public class AuthConfiguration
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigninKey { get; init; }
    public required string SourceEmail { get; init; }
    public required string SmtpUsername { get; init; }
    public required string SmtpPassword { get; init; }
    public required string GoogleClientId { get; init; }
    public required string GoogleClientSecret { get; init; }
    public required string FacebookAppId { get; init; }
    public required string FacebookAppSecret { get; init; }
}