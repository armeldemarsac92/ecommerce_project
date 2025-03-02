namespace Tdev702.Repository.SQL;

public static class ProductQueries
{
    public static string GetProductById = @"
    SELECT 
        *
    FROM shop.vw_full_product
    WHERE id = @Id;";
    
    public static string GetAllProducts = @"
    SELECT 
        *
    FROM shop.vw_products        
    ORDER BY 
        CASE WHEN @orderBy = 2 THEN id END DESC,
        CASE WHEN @orderBy = 1 THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";
    
    public static string GetProductsByIds = @"
    SELECT *
    FROM shop.vw_products
    WHERE id = ANY(@ProductIds);";
    
    public static string GetUserLikedProducts => @"
    SELECT p.*
    FROM shop.vw_products p
    INNER JOIN shop.link_products_users lp ON p.id = lp.product_id
    WHERE lp.user_id = @UserId
    ORDER BY 
        CASE WHEN @orderBy = 2 THEN id END DESC,
        CASE WHEN @orderBy = 1 THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";

    public static string CreateProduct = @"
    INSERT INTO shop.products (
        title,
        description,
        price,
        image_url,
        brand_id,
        category_id,
        open_food_fact_id,
        created_at,
        updated_at
    )
    VALUES (
        @Title,
        @Description,
        @Price,
        @ImageUrl,
        @BrandId,
        @CategoryId,
        @OpenFoodFactId,
        CURRENT_TIMESTAMP,
        CURRENT_TIMESTAMP
    )
    RETURNING id;";
    
    public static string UpdateProduct = @"
    UPDATE shop.products
    SET 
        image_url = COALESCE(@ImageUrl, image_url),
        title = COALESCE(@Title, title),
        description = COALESCE(@Description, description),
        price = COALESCE(@Price, price),
        brand_id = COALESCE(@BrandId, brand_id),
        category_id = COALESCE(@CategoryId, category_id),
        open_food_fact_id = COALESCE(@OpenFoodFactId, open_food_fact_id),
        updated_at = CURRENT_TIMESTAMP
    WHERE id = @Id;";

    public static string DeleteProduct = @"
    DELETE FROM shop.products
    WHERE id = @Id;";
    
    public static string InsertLike => @"
    INSERT INTO shop.link_products_users (user_id, product_id, updated_at, created_at)
    VALUES (@UserId, @ProductId, current_timestamp, current_timestamp);";

    public static string DeleteLike => @"
    DELETE FROM shop.link_products_users
    WHERE user_id = @UserId AND product_id = @ProductId;";
}