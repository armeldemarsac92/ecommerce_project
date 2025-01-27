namespace Tdev702.Repository.SQL;

public static class CategoryQueries
{
    public static string GetCategoryById = @"
    SELECT *
    FROM shop.categories
    WHERE id = @id;";

    public static string GetAllCategories = @"
    SELECT *
    FROM shop.categories
    ORDER BY 
        CASE WHEN @orderBy = 2 THEN id END DESC,
        CASE WHEN @orderBy = 1 THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";

    public static string CreateCategory = @"
    INSERT INTO shop.categories (title, description)
    VALUES (@title, @description)
    RETURNING id;";

    public static string UpdateCategory = @"
    UPDATE shop.categories
    SET title = COALESCE(@title, title),
    description = COALESCE(@description, description)
    WHERE id = @id;";

    public static string DeleteCategory = @"
    DELETE FROM shop.categories
    WHERE id = @id;";

}