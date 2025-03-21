using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Request.Tag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.All;
using static Tdev702.Contracts.Mapping.QueryOptionsMapping;

namespace Tdev702.Api.Endpoints;

public static class TagEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "ProductTags";
    
    public static IEndpointRouteBuilder MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.ProductsTags.GetAll, GetAllTags)
            .WithTags(Tags)
            .WithDescription("Get all products tags")
            .RequireAuthorization("Authenticated")
            .Produces<List<TagResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.ProductsTags.GetById, GetTagById)
            .WithTags(Tags)
            .WithDescription("Get one product tag")
            .RequireAuthorization("Authenticated")
            .Produces<TagResponse>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.ProductsTags.Create, CreateTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .RequireAuthorization("Admin")
            .Accepts<CreateTagRequest>(ContentType)
            .Produces<TagResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.ProductsTags.Update, UpdateTag)
            .WithTags(Tags)
            .WithDescription("Create one product tag")
            .RequireAuthorization("Admin")
            .Accepts<UpdateTagRequest>(ContentType)
            .Produces<TagResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.ProductsTags.Delete, DeleteTag)
            .WithTags(Tags)
            .WithDescription("Delete one product tag by Id")
            .RequireAuthorization("Admin")
            .Produces<TagResponse>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetTagById(
        HttpContext context,
        ITagsService tagsService,
        long id,
        CancellationToken cancellationToken)
    {   
        var productTag = await tagsService.GetByIdAsync(id ,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> GetAllTags(
        HttpContext context,
        ITagsService tagsService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);

        var productTags = await tagsService.GetAllAsync(queryOptions, cancellationToken);
        return Results.Ok(productTags);
    }

    private static async Task<IResult> CreateTag(
        HttpContext context,
        ITagsService tagsService,
        CreateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await tagsService.CreateAsync(tagRequest,cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> UpdateTag(
        HttpContext context,
        ITagsService tagsService,
        long id,
        UpdateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        var productTag = await tagsService.UpdateAsync(id, tagRequest, cancellationToken);
        return Results.Ok(productTag);
    }
    private static async Task<IResult> DeleteTag(
        HttpContext context,
        ITagsService tagsService,
        long id,
        CancellationToken cancellationToken)
    {
        await tagsService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }
}