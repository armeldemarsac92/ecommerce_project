namespace Tdev702.Repository.SQL;

public static class ProductQueries
{
    public static string GetProductById = @"
    SELECT 
        id,
        stripe_id,
        title,
        description,
        price,
        brand_id,
        category_id,
        open_food_fact_id,
        updated_at,
        created_at
    FROM shop.products
    WHERE id = @Id;";
    
    public static string GetAllProducts = @"
    SELECT 
        id,
        stripe_id,
        title,
        description,
        price,
        brand_id,
        category_id,
        open_food_fact_id,
        updated_at,
        created_at
    FROM shop.products;";

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
        NULLIF(@BrandId, 0),
        NULLIF(@CategoryId, 0),
        NULLIF(@OpenFoodFactId, 0),
        CURRENT_TIMESTAMP,
        CURRENT_TIMESTAMP
    )
    RETURNING id;";
    
    public static string UpdateProduct = @"
    UPDATE shop.products
    SET 
        stripe_id = @StripeId,
        title = @Title,
        description = @Description,
        price = @Price,
        brand_id = NULLIF(@BrandId, 0),
        category_id = NULLIF(@CategoryId, 0),
        open_food_fact_id = NULLIF(@OpenFoodFactId, 0),
        updated_at = CURRENT_TIMESTAMP
    WHERE id = @Id;";

    public static string DeleteProduct = @"
    DELETE FROM shop.products
    WHERE id = @Id;";
}