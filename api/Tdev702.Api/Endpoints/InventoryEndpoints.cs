using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Api.Endpoints;

public static class InventoryEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Inventories";

    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Inventories.GetById, GetInventory)
            .WithTags(Tags)
            .WithDescription("Get Inventory by id")
            .Produces<InventorySQLResponse>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.GetInventoryByProductId, GetInventoryByProductId)
            .WithTags(Tags)
            .WithDescription("Get Inventory by product id")
            .Produces<InventorySQLResponse>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.GetAll, GetAllInventory)
            .WithTags(Tags)
            .WithDescription("Get all inventories")
            .Produces<List<InventorySQLResponse>>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.Inventories.Create, CreateInventory)
            .WithTags(Tags)
            .WithDescription("Create new inventory")
            .Produces<InventorySQLResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Inventories.Update, UpdateInventory)
            .WithTags(Tags)
            .WithDescription("Update new inventory")
            .Produces<InventorySQLResponse>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Inventories.Delete, DeleteInventory)
            .WithTags(Tags)
            .WithDescription("Update new inventory")
            .Produces<InventorySQLResponse>(200)
            .Produces(404);

        app.MapPut(ShopRoutes.Inventories.IncreamentStockInventory, IncreamentStockInventory)
            .WithTags(Tags)
            .WithDescription("Increament stock inventory")
            .Accepts<UpdateQuantityRequest>(ContentType)
            .Produces<InventorySQLResponse>(200)
            .Produces(404);
        
        app.MapPut(ShopRoutes.Inventories.SubstractStockInventory, SubstractStockInventory)
            .WithTags(Tags)
            .WithDescription("Substract stock inventory")
            .Accepts<UpdateQuantityRequest>(ContentType)
            .Produces<InventorySQLResponse>(200)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        long id,
        CancellationToken cancellationToken)
    {   
        var inventory = await inventoriesService.GetByIdAsync(id ,cancellationToken);
        return Results.Ok(inventory);
    }
    
    private static async Task<IResult> GetInventoryByProductId(
        HttpContext context,
        IInventoriesService inventoriesService,
        long productId,
        CancellationToken cancellationToken)
    {   
        var inventory = await inventoriesService.GetByProductIdAsync(productId ,cancellationToken);
        return Results.Ok(inventory);
    }
    
    private static async Task<IResult> GetAllInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        CancellationToken cancellationToken,
        string? pageSize,
        string? pageNumber,
        string? sortBy)
    {
        var queryOptions = new QueryOptions
        {
            PageSize = int.TryParse(pageSize, out int size) ? size : 30,
            PageNumber = int.TryParse(pageNumber, out int page) ? page : 1,
            SortBy = Enum.TryParse<QueryOptions.Order>(sortBy, true, out var order) ? order : QueryOptions.Order.ASC
        };
        var inventory = await inventoriesService.GetAllAsync(queryOptions, cancellationToken);
        return Results.Ok(inventory);
    }
    
    private static async Task<IResult> CreateInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        CreateInventoryRequest createInventoryRequest,
        CancellationToken cancellationToken)
    {   
        var inventory = await inventoriesService.CreateAsync(createInventoryRequest, cancellationToken);
        return Results.Ok(inventory);
    }
    
    private static async Task<IResult> UpdateInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        long id,
        UpdateInventoryRequest updateInventoryRequest,
        CancellationToken cancellationToken)
    {   
        var inventory = await inventoriesService.UpdateAsync(id, updateInventoryRequest, cancellationToken);
        return Results.Ok(inventory);
    }
    
    private static async Task<IResult> DeleteInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        long id,
        CancellationToken cancellationToken)
    {   
        await inventoriesService.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> IncreamentStockInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        UpdateQuantityRequest updateQuantityRequest,
        long productId,
        CancellationToken cancellationToken)
    {
        await inventoriesService.IncreamentAsync(updateQuantityRequest.Quantity, productId, cancellationToken);
        return Results.NoContent();
    }
    
    private static async Task<IResult> SubstractStockInventory(
        HttpContext context,
        IInventoriesService inventoriesService,
        UpdateQuantityRequest updateQuantityRequest,
        long productId,
        CancellationToken cancellationToken)
    {
        await inventoriesService.SubstractAsync(updateQuantityRequest.Quantity, productId, cancellationToken);
        return Results.NoContent();
    }
}