using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.SQL.Request.Shop.Inventory;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Api.Endpoints;

public static class InventoryEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Invoices";

    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Inventories.GetById, GetInventory)
            .WithTags(Tags)
            .WithDescription("Get Inventory by id")
            .Produces<List<InventoryResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.GetInventoryByProductId, GetInventoryByProductId)
            .WithTags(Tags)
            .WithDescription("Get Inventory by product id")
            .Produces<List<InventoryResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.GetAll, GetAllInventory)
            .WithTags(Tags)
            .WithDescription("Get all inventories")
            .Produces<List<InventoryResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.Create, CreateInventory)
            .WithTags(Tags)
            .WithDescription("Create new inventory")
            .Produces<List<InventoryResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.Update, UpdateInventory)
            .WithTags(Tags)
            .WithDescription("Update new inventory")
            .Produces<List<InventoryResponse>>(200)
            .Produces(404);
        
        app.MapGet(ShopRoutes.Inventories.Delete, DeleteInventory)
            .WithTags(Tags)
            .WithDescription("Update new inventory")
            .Produces<List<InventoryResponse>>(200)
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
        CancellationToken cancellationToken)
    {   
        var inventory = await inventoriesService.GetAllAsync(cancellationToken);
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
}