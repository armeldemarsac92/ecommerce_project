using BrandResponse = Tdev702.Contracts.Response.Shop.BrandResponse;

namespace Tdev702.Contracts.Mapping;

public static class BrandMapping
{
    public static BrandResponse MapToBrand(this SQL.BrandResponse brandResponse)
    {
        return new BrandResponse
        {
            BrandId = brandResponse.BrandId,
            Title = brandResponse.Title,
            Description = brandResponse.Description,
        };
    }

    public static List<BrandResponse> MapToBrands(this List<SQL.BrandResponse> brandResponses)
    {
        return brandResponses.Select(MapToBrand).ToList();
    }
}