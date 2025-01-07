namespace Tdev702.Contracts.Config;

public class AuthConfiguration
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigninKey { get; init; }
}