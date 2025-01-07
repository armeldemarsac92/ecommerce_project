using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Request.Shop.ProductTag;

namespace Tdev702.Api.Endpoints.Shop;

public static class TagEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "ProductTags";
    
    public static IEndpointRouteBuilder MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.ProductsTags.GetAll, GetAllTags)
            .WithTags(Tags)
            .WithDescription("Get all products tags")
            .Produces<List<Tag>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.ProductsTags.GetById, GetTags)
            .WithTags(Tags)
            .WithDescription("Get one product tag")
            .Produces<Tag>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.ProductsTags.Create, CreateTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .Produces<Tag>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.ProductsTags.Update, UpdateTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .Produces<Tag>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.ProductsTags.Delete, DeleteTag)
            .WithTags(Tags)
            .WithDescription("Delete one product tag by Id")
            .Produces<Tag>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetTags(
        HttpContext context,
        ITagsService tagsService,
        long productTagId,
        CancellationToken cancellationToken)
    {   
        var productTag = await tagsService.GetByIdAsync(productTagId ,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> GetAllTags(
        HttpContext context,
        ITagsService tagsService,
        CancellationToken cancellationToken)
    {
        var productTags = await tagsService.GetAllAsync(cancellationToken);
        return Results.Ok(productTags);
    }

    private static async Task<IResult> CreateTag(
        HttpContext context,
        ITagsService tagsService,
        CreateTagRequest createTagRequest,
        CancellationToken cancellationToken)
    {
        var tag = await tagsService.CreateAsync(createTagRequest,cancellationToken);
        return Results.Ok(tag);
    }
    private static async Task<IResult> UpdateTag(
        HttpContext context,
        ITagsService tagsService,
        long tagId,
        UpdateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await tagsService.UpdateAsync(tagId, tagRequest, cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> DeleteTag(
        HttpContext context,
        ITagsService tagsService,
        long productTagId,
        CancellationToken cancellationToken)
    {
        await tagsService.DeleteAsync(productTagId, cancellationToken);
        return Results.NoContent();
    }
}