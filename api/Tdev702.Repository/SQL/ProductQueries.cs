namespace Tdev702.Repository.SQL;

public static class ProductQueries
{
    public static string GetProductById = @"
    SELECT *
    FROM shop.products
    WHERE id = @Id;";
    
    public static string GetAllProducts = @"
    SELECT *
    FROM shop.products;";

    public static string CreateProduct = @"
    INSERT INTO shop.products (stripe_id, title, description, price)
    VALUES (@stripeId, @title, @description, @price);";
    
    public static string UpdateProduct = @"
    UPDATE shop.products
    SET title = @title,
        description = @description,
        price = @price,
        brand_id = @brandId,
        category_id = @categoryId,
        open_food_fact_id = @openFoodFactId,
        updated_at = CURRENT_TIMESTAMP
    WHERE id = @Id;";

    public static string DeleteProduct = @"
    DELETE FROM shop.products
    Where id = @Id;";

}