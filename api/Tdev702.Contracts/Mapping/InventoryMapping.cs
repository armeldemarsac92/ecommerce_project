using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;
using InventoryResponse = Tdev702.Contracts.API.Shop.InventoryResponse;

namespace Tdev702.Contracts.SQL.Mapping;

public static class InventoryMapping
{
    public static InventoryResponse MapToProductInventory(this Response.Shop.InventoryResponse inventoryResponse)
    {
        return new InventoryResponse
        {
            Id = inventoryResponse.Id,
            Quantity = inventoryResponse.Quantity,
            Sku = inventoryResponse.Sku,
            CreatedAt = inventoryResponse.CreatedAt,
            UpdatedAt = inventoryResponse.UpdatedAt,
            ProductId = inventoryResponse.ProductId,
        };
    }
    
    public static List<InventoryResponse> MapToProductInventories(this List<Response.Shop.InventoryResponse> productResponses)
    {
        return productResponses.Select(MapToProductInventory).ToList();
    }
}