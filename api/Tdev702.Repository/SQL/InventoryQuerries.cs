namespace Tdev702.Repository.SQL;

public class InventoryQuerries
{
    public static string GetInventoryById = @"
    SELECT *
    FROM backoffice.inventory
    WHERE id = @id ";

    public static string GetInventoryByProductId = @"
    SELECT *
    FROM backoffice.inventory
    WHERE product_id = @product_id";

    public static string GetAllInventory = @"
    SELECT *
    From backoffice.inventory;";

    public static string CreateInventory = @"
    insert into backoffice.inventory(sku,quantity,product_id, created_at, updated_at)
    VALUES (@sku, @quantity, @product_id, current_timestamp, current_timestamp)
    RETURNING * ;";

    public static string UpdateInventory = @"
    UPDATE backoffice.inventory
    SET 
    sku = @sku,
    quantity = @quantity,
    updated_at = CURRENT_TIMESTAMP
    WHERE id = @id;";

    public static string DeleteInventory = @"
    DELETE FROM backoffice.inventory
    WHERE id = @id;";
}