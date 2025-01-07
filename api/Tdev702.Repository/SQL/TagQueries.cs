namespace Tdev702.Repository.SQL;

public static class TagQueries
{
    public static string GetProductTagById = @"
    SELECT *
    FROM shop.product_tags
    WHERE id = @ProductTagId;"; 

    public static string GetAllProductTags = @"
    SELECT *
    FROM shop.product_tags;";

    public static string CreateProductTag = @"
    INSERT INTO shop.product_tags (title, description)
    VALUES (@title, @description)
    RETURNING *;";

    public static string UpdateProductTag = @"
    UPDATE shop.product_tags
    SET 
    title = @Title,
    description = @Description
    WHERE id = @ProductTagId;";

    public static string DeleteProductTag = @"
    DELETE FROM shop.product_tags
    WHERE id = @ProductTagId;";
}