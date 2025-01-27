using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Request.Tag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Tag;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class TagMapping
{
    public static TagResponse MapToTag(this TagSQLResponse tagSqlResponse)
    {
        return new TagResponse()
        {
            Id = tagSqlResponse.Id,
            Title = tagSqlResponse.Title,
            Description = tagSqlResponse.Description,
        };
    }

    public static List<TagResponse> MapToTags(this List<TagSQLResponse> productTagResponses)
    {
        return productTagResponses.Select(MapToTag).ToList();
    }

    public static CreateTagSQLRequest MapToCreateTagRequest(this CreateTagRequest createTagRequest)
    {
        return new CreateTagSQLRequest()
        {
            Title = createTagRequest.Title,
            Description = createTagRequest.Description,
        };
    }

    public static UpdateTagSQLRequest MapToUpdateTagRequest(this UpdateTagRequest updateTagRequest, long tagId)
    {
        return new UpdateTagSQLRequest()
        {
            Id = tagId,
            Title = updateTagRequest.Title,
            Description = updateTagRequest.Description,
        };
    }

}