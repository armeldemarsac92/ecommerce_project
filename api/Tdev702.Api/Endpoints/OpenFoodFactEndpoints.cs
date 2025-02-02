using Microsoft.AspNetCore.Mvc;
using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.OpenFoodFact.Request;
using Tdev702.Contracts.OpenFoodFact.Response;

namespace Tdev702.Api.Endpoints;

public static class OpenFoodFactEndpoints
{
    public const string ContentType = "application/json";
    public const string Tags = "Open Food Facts";

    public static IEndpointRouteBuilder MapOpenFoodFactEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.OpenFoodFactProducts.GetAll, GetAllOpenFoodFactProducts)
            .WithTags(Tags)
            .RequireAuthorization("Admin")
            .WithDescription("Get all products from Open Food Facts")
            .Produces<OpenFoodFactSearchResult>(200);

        return app;

    }

    private static async Task<IResult> GetAllOpenFoodFactProducts(
        HttpContext context,
        [AsParameters] ProductSearchParams options,
        IOpenFoodFactService openFoodFactService,
        CancellationToken cancellationToken
    )
    {
        var response = await openFoodFactService.SearchProductAsync(options, cancellationToken);
        return Results.Ok(response);
    }
}