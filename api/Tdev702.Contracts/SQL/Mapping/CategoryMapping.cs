using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class CategoryMapping
{
    public static Category MapToCategory(this CategoryResponse categoryResponse)
    {
        return new Category
        {
            Id = categoryResponse.Id,
            Title = categoryResponse.Title,
            Description = categoryResponse.Description
        };
    }

    public static List<Category> MapToCategories (this List<CategoryResponse> categoryResponses)
    {
        return categoryResponses.Select(MapToCategory).ToList();
    }
}