using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Inventory;

public class CreateInventoryRequest
{
    [JsonPropertyName("product_id")]
    public required long ProductId { get; set; }

    [JsonPropertyName("quantity")] 
    public required long Quantity { get; set; }
   
    [JsonPropertyName("sku")]
    public string Sku { get; set; }
   
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
   
    [JsonPropertyName("created_at")]
    public required DateTimeOffset? CreatedAt { get; set; }
}