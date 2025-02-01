namespace Tdev702.Repository.SQL;

public class InventoryQueries
{
    public static string GetInventoryById = @"
    SELECT *
    FROM backoffice.inventory
    WHERE id = @id ";

    public static string GetInventoryByProductId = @"
    SELECT *
    FROM backoffice.inventory
    WHERE product_id = @product_id";

    public static string GetAllInventories = @"
    SELECT *
    From backoffice.inventory
    ORDER BY 
        CASE WHEN @orderBy = 2 THEN id END DESC,
        CASE WHEN @orderBy = 1 THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";

    public static string CreateInventory = @"
    insert into backoffice.inventory(sku,quantity,product_id, created_at, updated_at)
    VALUES (@sku, @quantity, @productId, current_timestamp, current_timestamp)
    RETURNING id;";

    public static string UpdateInventory = @"
    UPDATE backoffice.inventory
    SET 
    sku = COALESCE(@sku, sku),
    quantity = COALESCE (@quantity, quantity),
    updated_at = CURRENT_TIMESTAMP
    WHERE id = @id;";

    public static string DeleteInventory = @"
    DELETE FROM backoffice.inventory
    WHERE id = @id;";
}