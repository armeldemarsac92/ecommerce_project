namespace Tdev702.Repository.SQL;

public static class LinkProductTagQueries
{
    // A refaire avec les joins
    
    // public static string GetLinkByProductId = @"
    // SELECT * 
    // FROM shop.link_products_tags
    // WHERE product_id = @ProductId;";
    
    // public static string GetLinkByTagId = @"
    // SELECT * 
    // FROM shop.link_products_tags
    // WHERE tag_id = @TagId;";
    //
    // public static string GetLinkByTagAndProductId = @"
    // SELECT *
    // FROM shop.link_products_tags
    // WHERE tag_id = @TagId
    // AND product_id = @ProductId;";

    public static string CreateLinkProductTag = @"
    INSERT INTO shop.link_products_tags (product_id, tag_id)
    VALUES (@productId, @tagId)
    RETURNING *;";
    
    public static string DeleteLinkProductTag = @"
    DELETE FROM shop.link_products_tags
    WHERE id = @Id;"; 
    
    public static string GetLinkById = @"
    SELECT *
    FROM shop.link_products_tags
    WHERE id = @Id;";

    public static string GetAllLinks = @"
    SELECT *
    FROM shop.link_products_tags
";
}