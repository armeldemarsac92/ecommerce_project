using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Request.Shop.Product;

namespace Tdev702.Api.Endpoints.Shop;

public static class ProductEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Products";

    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Products.GetAll, GetAllProducts)
            .WithTags(Tags)
            .WithDescription("Get all products")
            .Produces<List<Product>>(200)
            .Produces(404);

        app.MapGet(ShopRoutes.Products.Create, CreateProduct)
            .WithTags(Tags)
            .WithDescription("Create product")
            .Produces(200)
            .Produces(404);
        
        return app;
        
    }

    private static async Task<IResult> GetAllProducts(
        HttpContext context,
        IProductsService productsService,
        CancellationToken cancellationToken)
    {
        var products = await productsService.GetAllAsync(cancellationToken);
        return Results.Ok(products);
    }

    private static async Task<IResult> CreateProduct(
        HttpContext context,
        IProductsService productsService,
        CreateProductRequest productRequest,
        CancellationToken cancellationToken)
    {
        var product = await productsService.CreateAsync(productRequest, cancellationToken);
        return Results.Ok(product);
    }
    
}