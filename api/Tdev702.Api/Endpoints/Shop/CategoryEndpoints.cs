using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Request.Shop.Category;

namespace Tdev702.Api.Endpoints.Shop;

public static class CategoryEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Categories";

    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Categories.GetAll, GetAllCategories)
            .WithTags(Tags)
            .WithDescription("Get all Categories")
            .Produces<List<Category>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Categories.GetById, GetCategory)
            .WithTags(Tags)
            .WithDescription("Get one Category")
            .Produces<Category>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.Categories.Create, CreateCategory)
            .WithTags(Tags)
            .WithDescription("Create one Category")
            .Produces<Category>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Categories.Update, UpdateCategory)
            .WithTags(Tags)
            .WithDescription("Update one Category")
            .Produces<Category>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Categories.Delete, DeleteCategory)
            .WithTags(Tags)
            .WithDescription("Delete one Category by Id")
            .Produces<Category>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetCategory(
        HttpContext context,
        ICategoriesService categoriesService,
        long categoryId,
        CancellationToken cancellationToken)
    {   
        var category = await categoriesService.GetByIdAsync(categoryId ,cancellationToken);
        return Results.Ok(category);
    }
    private static async Task<IResult> GetAllCategories(
        HttpContext context,
        ICategoriesService categoriesService,
        CancellationToken cancellationToken)
    {
        var categories = await categoriesService.GetAllAsync(cancellationToken);
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
        long categoryId,
        UpdateCategoryRequest categoryRequest,
        CancellationToken cancellationToken)
    {
        var category = await categoriesService.UpdateAsync(categoryId, categoryRequest, cancellationToken);
        return Results.Ok(category);
    }
    private static async Task<IResult> DeleteCategory(
        HttpContext context,
        ICategoriesService categoriesService,
        long categoryId,
        CancellationToken cancellationToken)
    {
        await categoriesService.DeleteAsync(categoryId, cancellationToken);
        return Results.NoContent();
    }

}