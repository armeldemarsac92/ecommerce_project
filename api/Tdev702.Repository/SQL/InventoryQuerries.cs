namespace Tdev702.Repository.SQL;

public class InventoryQuerries
{
    public static string GetInventoryById = @"
    SELECT *
    FROM shop.inventory
    WHERE id = @id ";

    public static string GetInventoryByProductId = @"
    SELECT *
    FROM shop.inventory
    WHERE product_id = @product_id";

    public static string GetAllInventory = @"
    SELECT *
    From shop.inventory;";

    public static string CreateInventory = @"
    insert into shop.inventory(sku,quantity,product_id, created_at)
    VALUES (@sku, @quantity, @product_id, current_timestamp)
    RETURNING * ;";

    public static string UpdateInventory = @"
    UPDATE shop.inventory
    SET 
    sku = @sku,
    quantity = @quantity,
    updated_at = CURRENT_TIMESTAMP
    WHERE id = @id;";
    
    public static string DeleteInventory = @"
    DELETE FROM shop.inventory
    WHERE id = @id;";
}