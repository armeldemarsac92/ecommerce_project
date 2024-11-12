using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class CategoryMapping
{
    public static Category MapToCategory(this CategoryResponse CategoryResponse)
    {
        return new Category
        {
            Id = CategoryResponse.Id,
            Title = CategoryResponse.Title,
            Description = CategoryResponse.Description
        };
    }

    public static List<Category> MapToCategories (this List<CategoryResponse> CategoryResponses)
    {
        return CategoryResponses.Select(MapToCategory).ToList();
    }
}