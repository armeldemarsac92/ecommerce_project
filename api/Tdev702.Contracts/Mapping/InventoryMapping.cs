using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class InventoryMapping
{
    public static InventoryResponse MapToProductInventory(this InventorySQLResponse inventoryResponse)
    {
        return new InventoryResponse()
        {
            Id = inventoryResponse.Id,
            Quantity = inventoryResponse.Quantity,
            Sku = inventoryResponse.Sku,
            CreatedAt = inventoryResponse.CreatedAt,
            UpdatedAt = inventoryResponse.UpdatedAt,
            ProductId = inventoryResponse.ProductId,
        };
    }
    
    public static List<InventoryResponse> MapToProductInventories(this List<InventorySQLResponse> inventorySqlResponses)
    {
        return inventorySqlResponses.Select(MapToProductInventory).ToList();
    }
}