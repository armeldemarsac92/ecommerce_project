namespace Tdev702.Auth.Models;

public class Setup2FaRequest
{
    public TwoFactorType Type { get; set; }
    public string? PhoneNumber { get; set; }  // Only needed for SMS
}