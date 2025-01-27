namespace Tdev702.Repository.SQL;

public static class ProductQueries
{
    public static string GetProductById = @"
    SELECT 
        *
    FROM shop.vw_products
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

    public static string CreateProduct = @"
    INSERT INTO shop.products (
        stripe_id,
        title,
        description,
        price,
        brand_id,
        category_id,
        open_food_fact_id,
        created_at,
        updated_at
    )
    VALUES (
        @StripeId,
        @Title,
        @Description,
        @Price,
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
        stripe_id = COALESCE(@StripeId, stripe_id),
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
}