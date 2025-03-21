using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Api.Utils;
using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.All;
using static Tdev702.Contracts.Mapping.QueryOptionsMapping;

namespace Tdev702.Api.Endpoints;

public static class ProductEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Products";

    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Products.GetAll, GetAllProducts)
            .WithTags(Tags)
            .WithDescription("Get all products")
            .Produces<List<ShopProductResponse>>(200)
            .Produces(204);
        
        app.MapGet(ShopRoutes.Products.GetById, GetProduct)
            .WithTags(Tags)
            .WithDescription("Get a product")
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Products.GetLiked, GetLikedProducts)
            .WithTags(Tags)
            .WithDescription("Get liked products")
            .RequireAuthorization("Authenticated")
            .Produces<List<ShopProductResponse>>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.Products.Create, CreateProduct)
            .WithTags(Tags)
            .WithDescription("Create a product")
            .RequireAuthorization("Admin")
            .Accepts<CreateProductRequest>(ContentType)
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Products.Update, UpdateProduct)
            .WithTags(Tags)
            .WithDescription("Update a product")
            .RequireAuthorization("Admin")
            .Accepts<UpdateProductRequest>(ContentType)
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Products.Delete, DeleteProduct)
            .WithTags(Tags)
            .WithDescription("Delete a product")
            .RequireAuthorization("Admin")
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Products.Like, LikeProduct)
            .WithTags(Tags)
            .WithDescription("Like a product")
            .RequireAuthorization("Authenticated")
            .Produces(404)
            .Produces(400);

        app.MapGet(ShopRoutes.Products.Unlike, UnlikeProduct)
            .WithTags(Tags)
            .WithDescription("Unlike a product ")
            .RequireAuthorization("Authenticated")
            .Produces(404)
            .Produces(400);
        
        app.MapGet(ShopRoutes.HealthCheck, () => Results.Ok("Healthy"))
            .WithName("HealthCheck")
            .WithTags(Tags);
        
        return app;
    }

    private static async Task<IResult> UnlikeProduct(
        HttpContext context,
        IProductsService productsService,
        CancellationToken cancellationToken,
        long id
    )
    {
        var userId = context.GetUserIdFromClaims();
        await productsService.UnlikeAsync(userId, id, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> LikeProduct(
        HttpContext context,
        IProductsService productsService,
        CancellationToken cancellationToken,
        long id
        )
    {
        var userId = context.GetUserIdFromClaims();
        await productsService.LikeAsync(userId, id, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> GetLikedProducts(
        HttpContext context,
        IProductsService productsService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);
        var userId = context.GetUserIdFromClaims();
        var likedProducts = await productsService.GetLikedProductsAsync(queryOptions, userId, cancellationToken);
        return Results.Ok(likedProducts);
    }

    private static async Task<IResult> GetAllProducts(
        HttpContext context,
        IProductsService productsService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);
        var userId = context.GetUserIdFromClaims();
        var products = await productsService.GetAllAsync(queryOptions, userId, cancellationToken);
        return Results.Ok(products);
    }
    
    private static async Task<IResult> GetProduct(
        HttpContext context,
        IProductsService productsService,
        long id,
        CancellationToken cancellationToken)
    {   
        var userId = context.GetUserIdFromClaims();
        var product = await productsService.GetByIdAsync(id, userId, cancellationToken);
        return Results.Ok(product);
    }
    
    private static async Task<IResult> CreateProduct(
        HttpContext context,
        IProductsService productsService,
        CreateProductRequest productRequest,
        CancellationToken cancellationToken)
    {
        var product = await productsService.CreateAsync(productRequest,cancellationToken);
        return Results.Ok(product);
    }
    private static async Task<IResult> UpdateProduct(
        HttpContext context,
        IProductsService productsService,
        long id,
        UpdateProductRequest productRequest,
        CancellationToken cancellationToken)
    {
        var product = await productsService.UpdateAsync(id, productRequest, cancellationToken);
        return Results.Ok(product);
    }
    private static async Task<IResult> DeleteProduct(
        HttpContext context,
        IProductsService productsService,
        long id,
        CancellationToken cancellationToken)
    {
        await productsService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }
}