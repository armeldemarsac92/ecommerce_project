using Tdev702.Api.Routes;
using Tdev702.Api.Services;
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
        
        app.MapPost(ShopRoutes.Products.Create, CreateProduct)
            .WithTags(Tags)
            .WithDescription("Create a product")
            .Accepts<CreateProductRequest>(ContentType)
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Products.Update, UpdateProduct)
            .WithTags(Tags)
            .WithDescription("Update a product")
            .Accepts<UpdateProductRequest>(ContentType)
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Products.Delete, DeleteProduct)
            .WithTags(Tags)
            .WithDescription("Delete a product")
            .Produces<ShopProductResponse>(200)
            .Produces(404);
        
        return app;
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

        var products = await productsService.GetAllAsync(queryOptions, cancellationToken);
        return Results.Ok(products);
    }
    
    private static async Task<IResult> GetProduct(
        HttpContext context,
        IProductsService productsService,
        long id,
        CancellationToken cancellationToken)
    {   
        var product = await productsService.GetByIdAsync(id ,cancellationToken);
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
        long productId,
        UpdateProductRequest productRequest,
        CancellationToken cancellationToken)
    {
        var product = await productsService.UpdateAsync(productId, productRequest, cancellationToken);
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