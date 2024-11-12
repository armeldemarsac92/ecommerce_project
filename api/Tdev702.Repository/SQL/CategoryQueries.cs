namespace Tdev702.Repository.SQL;

public class CategoryQueries
{
    public static string GetCategoryById = @"
    SELECT *
    FROM shop.categories
    WHERE id = @categoryId;";

    public static string GetAllCategories = @"
    SELECT *
    FROM shop.categories;";

    public static string CreateCategory = @"
    INSERT INTO shop.categories (title, description)
    VALUES (@title, @description);";

    public static string UpdateCategory = @"
    UPDATE shop.categories
    SET title = @title,
    description = @description
    WHERE id = @categoryId;";

    public static string DeleteCategory = @"
    DELETE FROM shop.categories
    WHERE id = @categoryId;";

}