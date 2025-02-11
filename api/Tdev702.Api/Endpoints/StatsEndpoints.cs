using Tdev702.Api.Routes;
using Tdev702.Api.Services;

namespace Tdev702.Api.Endpoints;

public static class StatEndpoints
{
    private const string Tags = "Stats";

    public static IEndpointRouteBuilder MapStatEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Stats.GetNewUsers, GetNewUsers)
            .WithTags(Tags)
            .WithDescription("Get new users daily or monthly")
            // .RequireAuthorization("Authenticated")
            .Produces(404);
        
        app.MapGet(ShopRoutes.Stats.GetCartAverage, GetCartAverage)
            .WithTags(Tags)
            .WithDescription("Get cart average daily or monthly")
            // .RequireAuthorization("Authenticated")
            .Produces(404);
        
        app.MapPost(ShopRoutes.Stats.GetRevenue, GetRevenue)
            .WithTags(Tags)
            .WithDescription("Get total revenue daily or monthly")
            // .RequireAuthorization("Admin")
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetNewUsers(
        IStatsService statsService,
        string dateRange,
        CancellationToken cancellationToken)
    {
        var stat = await statsService.GetNewUsersAsync(dateRange == "daily", cancellationToken);
        return Results.Ok(stat);
    }
    
    private static async Task<IResult> GetCartAverage(
        IStatsService statsService,
        string dateRange,
        CancellationToken cancellationToken)
    {
        var stat = await statsService.GetCartAverageAsync(dateRange == "daily", cancellationToken);
        return Results.Ok(stat);
    }

    private static async Task<IResult> GetRevenue(
        IStatsService statsService,
        string dateRange,
        CancellationToken cancellationToken)
    {
        var stat = await statsService.GetRevenueAsync(dateRange == "daily", cancellationToken);
        return Results.Ok(stat);
    }

}