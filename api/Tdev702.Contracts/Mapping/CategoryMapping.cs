using CategoryResponse = Tdev702.Contracts.Response.Shop.CategoryResponse;

namespace Tdev702.Contracts.Mapping;

public static class CategoryMapping
{
    public static CategoryResponse MapToCategory(this SQL.CategoryResponse categoryResponse)
    {
        return new CategoryResponse
        {
            Id = categoryResponse.Id,
            Title = categoryResponse.Title,
            Description = categoryResponse.Description
        };
    }

    public static List<CategoryResponse> MapToCategories (this List<SQL.CategoryResponse> categoryResponses)
    {
        return categoryResponses.Select(MapToCategory).ToList();
    }
}