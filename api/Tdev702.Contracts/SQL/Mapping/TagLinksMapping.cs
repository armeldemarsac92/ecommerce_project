using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class TagLinksMapping
{
    public static TagLinks MapToTagLink(this TagLinksResponse tagLinksResponse)
    {
        return new TagLinks()
        {
            Id = tagLinksResponse.Id,
            TagId = tagLinksResponse.TagId,
            ProductId = tagLinksResponse.ProductId,
        };
    }

    public static List<TagLinks> MapToTagLinks(this List<TagLinksResponse> tagLinksResponses)
    {
        return tagLinksResponses.Select(MapToTagLink).ToList();
    }
}