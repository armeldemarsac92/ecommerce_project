using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Brand;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class BrandMapping
{
    public static BrandResponse MapToBrand(this BrandSQLResponse brandSqlResponse)
    {
        return new BrandResponse
        {
            BrandId = brandSqlResponse.BrandId,
            Title = brandSqlResponse.Title,
            Description = brandSqlResponse.Description,
        };
    }

    public static List<BrandResponse> MapToBrands(this List<BrandSQLResponse> brandResponses)
    {
        return brandResponses.Select(MapToBrand).ToList();
    }

    public static CreateBrandSQLRequest MapToCreateBrandRequest(this CreateBrandRequest createBrandRequest)
    {
        return new CreateBrandSQLRequest
        {
            Title = createBrandRequest.Title,
            Description = createBrandRequest.Description
        };
    }

    public static UpdateBrandSQLRequest MapToUpdateBrandRequest(this UpdateBrandRequest updateBrandRequest, long brandId)
    {
        return new UpdateBrandSQLRequest
        {
            BrandId = brandId,
            Title = updateBrandRequest.Title,
            Description = updateBrandRequest.Description
        };
    }
}