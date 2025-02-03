using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Api.Utils;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Request.All;
using static Tdev702.Contracts.Mapping.QueryOptionsMapping;

namespace Tdev702.Api.Endpoints;

public static class OrderEndpoints
{
   private const string ContentType = "application/json";
   private const string Tags = "Orders";

   public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
   {
       app.MapGet(ShopRoutes.Orders.GetById, GetOrder)
           .WithTags(Tags)
           .WithDescription("Get one Order")
           .RequireAuthorization("Admin")
           .Produces<OrderResponse>(200)
           .Produces(404);

       app.MapGet(ShopRoutes.Orders.GetUserOrders, GetUserOrders)
           .WithTags(Tags)
           .WithDescription("Get all Orders for a User")
           .RequireAuthorization("Authenticated")
           .Produces<List<OrderResponse>>(200)
           .Produces(404);
       
       app.MapGet(ShopRoutes.Orders.GetAll, GetAllOrders)
           .WithTags(Tags)
           .WithDescription("Get all Orders by Status")
           .RequireAuthorization("Admin")
           .Produces<List<OrderResponse>>(200)
           .Produces(204);

       app.MapPost(ShopRoutes.Orders.Create, CreateOrder)
           .WithTags(Tags)
           .WithDescription("Create one Order")
           .RequireAuthorization("Authenticated")
           .Accepts<CreateOrderRequest>(ContentType)
           .Produces<OrderResponse>(201)
           .Produces(404);

       app.MapPut(ShopRoutes.Orders.Update, UpdateOrder)
           .WithTags(Tags)
           .WithDescription("Update one Order")
           .RequireAuthorization("Authenticated")
           .Accepts<UpdateOrderRequest>(ContentType)
           .Produces<OrderResponse>(200)
           .Produces(404);

       return app;
   }

   private static async Task<IResult> GetOrder(
       HttpContext context,
       IOrderService orderService,
       long id,
       CancellationToken cancellationToken)
   {
       var order = await orderService.GetByIdAsync(id, cancellationToken);
       return Results.Ok(order);
   }

   private static async Task<IResult> GetAllOrders(
       IOrderService orderService,
       CancellationToken cancellationToken,
       string? pageSize,
       string? pageNumber,
       string? sortBy)
   {
       var queryOptions = MapToQueryOptions(pageSize, pageNumber, sortBy);

       var orders = await orderService.GetAllAsync(queryOptions, cancellationToken);
       return Results.Ok(orders);
   }

   private static async Task<IResult> GetUserOrders(
       HttpContext context,
       IOrderService orderService,
       CancellationToken cancellationToken,
       string? pageSize,
       string? pageNumber,
       string? sortBy)
   {
       var queryOptions = new QueryOptions
       {
           PageSize = int.TryParse(pageSize, out int size) ? size : 30,
           PageNumber = int.TryParse(pageNumber, out int page) ? page : 1,
           OrderBy = Enum.TryParse<QueryOptions.Order>(sortBy, true, out var order) ? order : QueryOptions.Order.ASC
       };
       
       var userId = context.GetUserIdFromClaims();
       var orders = await orderService.GetAllByUserIdAsync(userId, queryOptions, cancellationToken);
       return Results.Ok(orders);
   }

   private static async Task<IResult> CreateOrder(
       HttpContext context,
       IOrderService orderService,
       CreateOrderRequest orderRequest,
       CancellationToken cancellationToken)
   {
       var order = await orderService.CreateAsync(orderRequest, cancellationToken);
       return Results.Created($"/api/orders/{order.Id}", order);
   }

   private static async Task<IResult> UpdateOrder(
       HttpContext context,
       IOrderService orderService,
       long orderId,
       UpdateOrderRequest orderRequest,
       CancellationToken cancellationToken)
   {
       var order = await orderService.UpdateAsync(orderId, orderRequest, cancellationToken);
       return Results.Ok(order);
   }
}