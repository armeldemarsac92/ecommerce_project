using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.All;
using static Tdev702.Contracts.Mapping.QueryOptionsMapping;

namespace Tdev702.Api.Endpoints;

public static class CategoryEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Categories";

    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Categories.GetAll, GetAllCategories)
            .WithTags(Tags)
            .WithDescription("Get all Categories")
            .Produces<List<CategoryResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Categories.GetById, GetCategory)
            .WithTags(Tags)
            .WithDescription("Get one Category")
            .Produces<CategoryResponse>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.Categories.Create, CreateCategory)
            .WithTags(Tags)
            .WithDescription("Create one Category")
            .Accepts<CreateCategoryRequest>(ContentType)
            .Produces<CategoryResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Categories.Update, UpdateCategory)
            .WithTags(Tags)
            .WithDescription("Update one Category")
            .Accepts<UpdateCategoryRequest>(ContentType)
            .Produces<CategoryResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Categories.Delete, DeleteCategory)
            .WithTags(Tags)
            .WithDescription("Delete one Category by Id")
            .Produces<CategoryResponse>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetCategory(
        HttpContext context,
        ICategoriesService categoriesService,
        long id,
        CancellationToken cancellationToken)
    {   
        var category = await categoriesService.GetByIdAsync(id ,cancellationToken);
        return Results.Ok(category);
    }
    private static async Task<IResult> GetAllCategories(
        HttpContext context,
        ICategoriesService categoriesService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);
        
        var categories = await categoriesService.GetAllAsync(queryOptions, cancellationToken);
        return Results.Ok(categories);
    }

    private static async Task<IResult> CreateCategory(
        HttpContext context,
        ICategoriesService categoriesService,
        CreateCategoryRequest categoryRequest,
        CancellationToken cancellationToken)
    {
        var category = await categoriesService.CreateAsync(categoryRequest,cancellationToken);
        return Results.Ok(category);
    }
    private static async Task<IResult> UpdateCategory(
        HttpContext context,
        ICategoriesService categoriesService,
        long id,
        UpdateCategoryRequest categoryRequest,
        CancellationToken cancellationToken)
    {
        var category = await categoriesService.UpdateAsync(id, categoryRequest, cancellationToken);
        return Results.Ok(category);
    }
    private static async Task<IResult> DeleteCategory(
        HttpContext context,
        ICategoriesService categoriesService,
        long id,
        CancellationToken cancellationToken)
    {
        await categoriesService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }

}