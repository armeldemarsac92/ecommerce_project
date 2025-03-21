using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.All;
using static Tdev702.Contracts.Mapping.QueryOptionsMapping;

namespace Tdev702.Api.Endpoints;

public static class BrandEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Brands";

    public static IEndpointRouteBuilder MapBrandEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Brands.GetAll, GetAllBrands)
            .WithTags(Tags)
            .WithDescription("Get all brands")
            .RequireAuthorization("Authenticated")
            .Produces<List<BrandResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Brands.GetById, GetBrand)
            .WithTags(Tags)
            .WithDescription("Get one brand")
            .RequireAuthorization("Authenticated")
            .Produces<BrandResponse>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.Brands.Create, CreateBrand)
            .WithTags(Tags)
            .WithDescription("Create one brand")
            .RequireAuthorization("Admin")
            .Accepts<CreateBrandRequest>(ContentType)
            .Produces<BrandResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Brands.Update, UpdateBrand)
            .WithTags(Tags)
            .WithDescription("Create one brand")
            .RequireAuthorization("Admin")
            .Accepts<UpdateBrandRequest>(ContentType)
            .Produces<BrandResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Brands.Delete, DeleteBrand)
            .WithTags(Tags)
            .WithDescription("Delete one brand by Id")
            .RequireAuthorization("Admin")
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetBrand(
        IBrandsService brandsService,
        long id,
        CancellationToken cancellationToken)
    {   
        var brand = await brandsService.GetByIdAsync(id ,cancellationToken);
        return Results.Ok(brand);
    }
    private static async Task<IResult> GetAllBrands(
        IBrandsService brandsService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);
        
        var brands = await brandsService.GetAllAsync(queryOptions, cancellationToken);
        return Results.Ok(brands);
    }

    private static async Task<IResult> CreateBrand(
        IBrandsService brandsService,
        CreateBrandRequest brandRequest,
        CancellationToken cancellationToken)
    {
        var brand = await brandsService.CreateAsync(brandRequest,cancellationToken);
        return Results.Ok(brand);
    }
    private static async Task<IResult> UpdateBrand(
        IBrandsService brandsService,
        long id,
        UpdateBrandRequest brandRequest,
        CancellationToken cancellationToken)
    {
        var brand = await brandsService.UpdateAsync(id, brandRequest, cancellationToken);
        return Results.Ok(brand);
    }
    private static async Task<IResult> DeleteBrand(
        IBrandsService brandsService,
        long id,
        CancellationToken cancellationToken)
    {
        await brandsService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }

}