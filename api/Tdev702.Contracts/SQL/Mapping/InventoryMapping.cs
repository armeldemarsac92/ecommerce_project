using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class InventoryMapping
{
    public static Inventory MapToProductInventory(this InventoryResponse inventoryResponse)
    {
        return new Inventory
        {
            Id = inventoryResponse.Id,
            Quantity = inventoryResponse.Quantity,
            Sku = inventoryResponse.Sku,
            CreatedAt = inventoryResponse.CreatedAt,
            UpdatedAt = inventoryResponse.UpdatedAt,
            ProductId = inventoryResponse.ProductId,
        };
    }
    
    public static List<Inventory> MapToProductInventories(this List<InventoryResponse> productResponses)
    {
        return productResponses.Select(MapToProductInventory).ToList();
    }
}