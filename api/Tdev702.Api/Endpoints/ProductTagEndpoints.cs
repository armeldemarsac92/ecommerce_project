using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.Endpoints;

public static class ProductTagEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "ProductTags";
    
    public static IEndpointRouteBuilder MapProductTagEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.ProductsTags.GetAll, GetAllProductTags)
            .WithTags(Tags)
            .WithDescription("Get all products tags")
            .Produces<List<ProductTagResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.ProductsTags.GetById, GetProductTags)
            .WithTags(Tags)
            .WithDescription("Get one product tag")
            .Produces<ProductTagResponse>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.ProductsTags.Create, CreateProductTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .Produces<ProductTagResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.ProductsTags.Update, UpdateProductTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .Produces<ProductTagResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.ProductsTags.Delete, DeleteProductTag)
            .WithTags(Tags)
            .WithDescription("Delete one product tag by Id")
            .Produces<ProductTagResponse>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetProductTags(
        HttpContext context,
        IProductTagsService productTagsService,
        long productTagId,
        CancellationToken cancellationToken)
    {   
        var productTag = await productTagsService.GetByIdAsync(productTagId ,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> GetAllProductTags(
        HttpContext context,
        IProductTagsService productTagsService,
        CancellationToken cancellationToken)
    {
        var productTags = await productTagsService.GetAllAsync(cancellationToken);
        return Results.Ok(productTags);
    }

    private static async Task<IResult> CreateProductTag(
        HttpContext context,
        IProductTagsService productTagsService,
        CreateProductTagRequest productTagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await productTagsService.CreateAsync(productTagRequest,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> UpdateProductTag(
        HttpContext context,
        IProductTagsService productTagsService,
        long productTagId,
        UpdateProductTagRequest productTagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await productTagsService.UpdateAsync(productTagId, productTagRequest, cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> DeleteProductTag(
        HttpContext context,
        IProductTagsService productTagsService,
        long productTagId,
        CancellationToken cancellationToken)
    {
        await productTagsService.DeleteAsync(productTagId, cancellationToken);
        return Results.NoContent();
    }
}