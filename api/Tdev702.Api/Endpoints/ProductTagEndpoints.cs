using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Request.Tag;
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
            .Produces<List<TagResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.ProductsTags.GetById, GetProductTags)
            .WithTags(Tags)
            .WithDescription("Get one product tag")
            .Produces<TagResponse>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.ProductsTags.Create, CreateProductTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .Produces<TagResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.ProductsTags.Update, UpdateProductTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .Produces<TagResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.ProductsTags.Delete, DeleteProductTag)
            .WithTags(Tags)
            .WithDescription("Delete one product tag by Id")
            .Produces<TagResponse>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetProductTags(
        HttpContext context,
        ITagsService tagsService,
        long productTagId,
        CancellationToken cancellationToken)
    {   
        var productTag = await tagsService.GetByIdAsync(productTagId ,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> GetAllProductTags(
        HttpContext context,
        ITagsService tagsService,
        CancellationToken cancellationToken)
    {
        var productTags = await tagsService.GetAllAsync(cancellationToken);
        return Results.Ok(productTags);
    }

    private static async Task<IResult> CreateProductTag(
        HttpContext context,
        ITagsService tagsService,
        CreateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await tagsService.CreateAsync(tagRequest,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> UpdateProductTag(
        HttpContext context,
        ITagsService tagsService,
        long productTagId,
        UpdateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await tagsService.UpdateAsync(productTagId, tagRequest, cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> DeleteProductTag(
        HttpContext context,
        ITagsService tagsService,
        long productTagId,
        CancellationToken cancellationToken)
    {
        await tagsService.DeleteAsync(productTagId, cancellationToken);
        return Results.NoContent();
    }
}