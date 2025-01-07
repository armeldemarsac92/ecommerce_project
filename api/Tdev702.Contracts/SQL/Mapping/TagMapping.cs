using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class TagMapping
{
    public static Tag MapToTag(this TagResponse tagResponse)
    {
        return new Tag()
        {
            TagId = tagResponse.TagId,
            Title = tagResponse.Title,
            Description = tagResponse.Description,
        };
    }

    public static List<Tag> MapToTags(this List<TagResponse> productTagResponses)
    {
        return productTagResponses.Select(MapToTag).ToList();
    }
}