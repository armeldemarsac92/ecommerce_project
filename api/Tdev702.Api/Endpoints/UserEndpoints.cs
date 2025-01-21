using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Api.Endpoints;

public static class UserEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Brands";

    public static IEndpointRouteBuilder MapGetUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Users.GetAll, GetAll)
            .WithTags(Tags)
            .WithDescription("Get all users")
            .Produces<UserSQLResponse>(200)
            .Produces(404);

        app.MapGet(ShopRoutes.Users.GetUserByEmail, GetByEmail)
            .WithTags(Tags)
            .WithDescription("Get user by email")
            .Produces<UserSQLResponse>(200)
            .Produces(400);

        app.MapGet(ShopRoutes.Users.GetById, GetById)
            .WithTags(Tags)
            .WithDescription("Get user by id")
            .Produces<UserSQLResponse>(200)
            .Produces(404);

        return app;
    }

    private static async Task<IResult> GetAll
    (
        HttpContext context,
        IUserService userService,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetAllUsersAsync(cancellationToken);
        return Results.Ok(user);
    }
    
    private static async Task<IResult> GetByEmail
    (
        HttpContext context,
        IUserService userService,
        string email,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByEmailAsync(email, cancellationToken);
        return Results.Ok(user);
    }
    
    private static async Task<IResult> GetById
    (
        HttpContext context,
        IUserService userService,
        string id,
        CancellationToken cancellationToken)
    {
        var user = await userService.UserExistsAsync(id, cancellationToken);
        return Results.Ok(user);
    }
}