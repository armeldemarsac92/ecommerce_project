namespace Tdev702.Contracts.Messaging;

public class TwoFactorCodeTask {
    public required string Email { get; init; }
    public required string Code { get; init; }
}