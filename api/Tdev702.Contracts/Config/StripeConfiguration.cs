namespace Tdev702.Contracts.Config;

public class StripeConfiguration
{
  public required string ApiKey { get; set; }
  public required string AccountWebhookSecret { get; set; }
  public required string PersonWebhookSecret { get; set; }
}