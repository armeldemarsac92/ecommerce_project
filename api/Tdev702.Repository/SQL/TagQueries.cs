namespace Tdev702.Repository.SQL;

public class TagQueries
{
    public static string GetTagById = @"
    SELECT *
    FROM shop.tags
    WHERE id = @TagId;"; 

    public static string GetAllTags = @"
    SELECT *
    FROM shop.tags        
    ORDER BY 
        CASE WHEN @orderBy = 'DESC' THEN id END DESC,
        CASE WHEN @orderBy = 'ASC' THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";
    
    public static string GetByIds = @"
    SELECT *
    FROM shop.tags
    WHERE id = ANY(@TagIds);";

    public static string CreateTag = @"
    INSERT INTO shop.tags (title, description)
    VALUES (@title, @description)
    RETURNING id;";

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