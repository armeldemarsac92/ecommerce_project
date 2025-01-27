using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.All;
using static Tdev702.Contracts.Mapping.QueryOptionsMapping;

namespace Tdev702.Api.Endpoints;

public static class CustomerEndpoints
{
    private const string Tags = "Customers";

    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Customers.GetAll, GetAllCustomers)
            .WithTags(Tags)
            .WithDescription("Get all Customers")
            .RequireAuthorization("Admin")
            .Produces<List<CustomerResponse>>(200)
            .Produces(404);

        app.MapGet(ShopRoutes.Customers.GetById, GetCustomer)
            .WithTags(Tags)
            .WithDescription("Get one Customer")
            .RequireAuthorization("Admin")
            .Produces<CustomerResponse>(200)
            .Produces(404);

        return app;
    }
    
    private static async Task<IResult> GetAllCustomers(
        ICustomerService customersService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);
        
        var customers = await customersService.GetAllAsync(queryOptions, cancellationToken);
        return Results.Ok(customers);
    }

    private static async Task<IResult> GetCustomer(
        ICustomerService customersService,
        string id,
        CancellationToken cancellationToken)
    {
        var customer = await customersService.GetByIdAsync(id, cancellationToken);
        return Results.Ok(customer);
    }

}