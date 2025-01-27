namespace Tdev702.Repository.SQL;

public class ProductTagQueries
{
    public static string GetProductTagById = @"
    SELECT *
    FROM shop.link_products_tags
    WHERE id = @ProductTagId;"; 
    
    public static string GetAllProductTagsByProductId = @"
    SELECT *
    FROM shop.link_products_tags
    WHERE product_id = @ProductId;";
    
    public static string GetAllProductTagsByTagId = @"
    SELECT *
    FROM shop.link_products_tags
    WHERE tag_id = @TagId;";

    public static string GetAllProductTags = @"
    SELECT *
    FROM shop.link_products_tags;";

    public static string CreateProductTag = @"
    INSERT INTO shop.link_products_tags (product_id, tag_id)
    VALUES (@ProductId, @TagId)
    RETURNING *;";

    public static string DeleteProductTagByProductId = @"
    DELETE FROM shop.link_products_tags
    WHERE product_id = @ProductId;";
    
    public static string DeleteProductTagByTagId = @"
    DELETE FROM shop.link_products_tags
    WHERE tag_id = @TagId;";
}