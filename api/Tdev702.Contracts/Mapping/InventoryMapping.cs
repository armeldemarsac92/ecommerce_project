using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.SQL.Mapping;

public static class InventoryMapping
{
    public static InventoryResponse MapToInventory(this InventorySQLResponse inventoryResponse)
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
    
    public static List<InventoryResponse> MapToInventories(this List<InventorySQLResponse> inventorySqlResponses)
    {
        return inventorySqlResponses.Select(MapToInventory).ToList();
    }

    public static UpdateInventorySQLRequest MapToInventoryRequest(this UpdateInventoryRequest updateInventoryRequest)
    {
        return new UpdateInventorySQLRequest()
        {
            Id = updateInventoryRequest.Id,
            Quantity = updateInventoryRequest.Quantity,
            Sku = updateInventoryRequest.Sku,
            CreatedAt = updateInventoryRequest.CreatedAt,
            UpdatedAt = updateInventoryRequest.UpdatedAt,
        };
    }
}