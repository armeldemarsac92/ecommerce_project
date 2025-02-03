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

public class IdentityProvider
{
    public required string Name { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string RedirectUri { get; init; }
    public required string Scope { get; init; }
    public required string FlowType { get; init; }
    public string? CodeEndpoint { get; init; }
    public required string TokenEndpointName { get; init; }
    public required string TokenEndpoint { get; init; }
    public required string UserInfoEndpointName { get; init; }
    public required string UserInfoEndpoint { get; init; }
}