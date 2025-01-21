namespace Tdev702.Repository.SQL;

public class TagQueries
{
    public static string GetTagById = @"
    SELECT *
    FROM shop.tags
    WHERE id = @TagId;"; 

    public static string GetAllTags = @"
    SELECT *
    FROM shop.tags;";

    public static string CreateTag = @"
    INSERT INTO shop.tags (title, description)
    VALUES (@title, @description)
    RETURNING *;";

    public static string UpdateTag = @"
    UPDATE shop.tags
    SET 
    title = COALESCE(@Title, title),
    description = COALESCE(@Description, description)
    WHERE id = @TagId;";

    public static string DeleteTag = @"
    DELETE FROM shop.tags
    WHERE id = @TagId;";
}