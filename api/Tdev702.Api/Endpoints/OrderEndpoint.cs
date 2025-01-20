using Microsoft.AspNetCore.Identity;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;

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
           .RequireAuthorization("Authenticated")
           .Produces<OrderResponse>(200)
           .Produces(404);

       app.MapGet(ShopRoutes.Orders.GetAll, GetOrdersByUserId)
           .WithTags(Tags)
           .WithDescription("Get all Orders for a User")
           .RequireAuthorization("Authenticated")
           .Produces<List<OrderResponse>>(200)
           .Produces(404);

       app.MapPost(ShopRoutes.Orders.Create, CreateOrder)
           .WithTags(Tags)
           .WithDescription("Create one Order")
           .RequireAuthorization("Authenticated")
           .Produces<OrderResponse>(201)
           .Produces(404);

       app.MapPut(ShopRoutes.Orders.Update, UpdateOrder)
           .WithTags(Tags)
           .WithDescription("Update one Order")
           .RequireAuthorization("Authenticated")
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
       HttpContext context,
       IOrderService orderService,
       CancellationToken cancellationToken)
   {
       var orders = await orderService.GetAllAsync(cancellationToken);
       return Results.Ok(orders);
   }

   private static async Task<IResult> GetOrdersByUserId(
       HttpContext context,
       IOrderService orderService,
       UserManager<User> userManager,
       CancellationToken cancellationToken)
   {
       var user = await userManager.GetUserAsync(context.User);
       if (user == null) throw new BadRequestException("Unknown user");
       var orders = await orderService.GetAllByUserIdAsync(user.Id, cancellationToken);
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
       long id,
       UpdateOrderRequest orderRequest,
       CancellationToken cancellationToken)
   {
       orderRequest.Id = id; 
       var order = await orderService.UpdateAsync(orderRequest, cancellationToken);
       return Results.Ok(order);
   }
}