using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.SQL.Request.Category;
using Tdev702.Contracts.SQL.Response;
using CategoryResponse = Tdev702.Contracts.API.Response.CategoryResponse;

namespace Tdev702.Contracts.Mapping;

public static class CategoryMapping
{
    public static CategoryResponse MapToCategory(this CategorySQLResponse categorySqlResponse)
    {
        return new CategoryResponse
        {
            Id = categorySqlResponse.Id,
            Title = categorySqlResponse.Title,
            Description = categorySqlResponse.Description
        };
    }

    public static List<CategoryResponse> MapToCategories (this List<CategorySQLResponse> categoryResponses)
    {
        return categoryResponses.Select(MapToCategory).ToList();
    }

    public static CreateCategorySQLRequest MapToCreateCategoryRequest(this CreateCategoryRequest createCategoryRequest)
    {
        return new CreateCategorySQLRequest
        {
            Title = createCategoryRequest.Title,
            Description = createCategoryRequest.Description
        };
    }

    public static UpdateCategorySQLRequest MapToUpdateCategoryRequest(this UpdateCategoryRequest updateCategoryRequest, long categoryId)
    {
        return new UpdateCategorySQLRequest
        {
            Id = categoryId,
            Title = updateCategoryRequest.Title,
            Description = updateCategoryRequest.Description
        };
    }
}