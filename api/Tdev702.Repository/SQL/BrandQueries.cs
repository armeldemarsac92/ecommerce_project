namespace Tdev702.Repository.SQL;

public static class BrandQueries
{
    public static string GetBrandById = @"
    SELECT *
    FROM shop.brands
    WHERE id = @BrandId;"; 

    public static string GetAllBrands = @"
    SELECT *
    FROM shop.brands;";

    public static string CreateBrand = @"
    INSERT INTO shop.brands (title, description)
    VALUES (@title, @description)
    RETURNING *;";

    public static string UpdateBrand = @"
    UPDATE shop.brands
    SET 
        title = COALESCE(@Title, title),
        description = COALESCE(@Description, description)
    WHERE id = @BrandId;";

    public static string DeleteBrand = @"
    DELETE FROM shop.brands
    WHERE id = @BrandId;";
}