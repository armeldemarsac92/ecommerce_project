using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class BrandMapping
{
    public static Brand MapToBrand(this BrandResponse brandResponse)
    {
        return new Brand
        {
            BrandId = brandResponse.BrandId,
            Title = brandResponse.Title,
            Description = brandResponse.Description,
        };
    }

    public static List<Brand> MapToBrands(this List<BrandResponse> brandResponses)
    {
        return brandResponses.Select(MapToBrand).ToList();
    }
}