using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Inventory;

public class UpdateQuantityRequest
{
    [JsonPropertyName("quantity")]
    public required int Quantity { get; init; }
}