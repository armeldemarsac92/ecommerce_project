namespace Tdev702.Contracts.Response.Shop;

public record PaymentMethodResponse
{
    public string Id { get; init; }
    public string Last4 { get; init; }
    public string Brand { get; init; }
    public long ExpiryMonth { get; init; }
    public long ExpiryYear { get; init; }
}