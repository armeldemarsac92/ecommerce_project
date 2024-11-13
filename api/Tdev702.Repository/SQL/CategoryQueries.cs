namespace Tdev702.Repository.SQL;

public static class CategoryQueries
{
    public static string GetCategoryById = @"
    SELECT *
    FROM shop.categories
    WHERE id = @id;";

    public static string GetAllCategories = @"
    SELECT *
    FROM shop.categories;";

    public static string CreateCategory = @"
    INSERT INTO shop.categories (title, description)
    VALUES (@title, @description)
    RETURNING *;";

    public static string UpdateCategory = @"
    UPDATE shop.categories
    SET title = @title,
    description = @description
    WHERE id = @id;";

    public static string DeleteCategory = @"
    DELETE FROM shop.categories
    WHERE id = @id;";

}