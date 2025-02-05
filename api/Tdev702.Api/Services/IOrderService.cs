using Stripe;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Request.Payment;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Services;

public interface IOrderService
{
    Task<OrderSummaryResponse> GetByIdAsync(long orderId, CancellationToken cancellationToken = default);
    Task<OrderSummaryResponse> GetOrderByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);
    Task<List<OrderSummaryResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    Task<List<OrderSummaryResponse>> GetAllByUserIdAsync(string userId, QueryOptions queryOptions, CancellationToken cancellationToken = default);
    Task<OrderSummaryResponse> CreateAsync(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken = default);
    Task UpdateOrderPaymentStatus(UpdateOrderSQLRequest updateOrderRequest,
        CancellationToken cancellationToken = default);
    Task<OrderSummaryResponse> UpdateAsync(long orderId, UpdateOrderRequest updateOrderRequest, CancellationToken cancellationToken = default);
    Task DeleteAsync(long orderId, CancellationToken cancellationToken = default);
    Task<PaymentIntent> CreatePaymentAsync(long orderId, string userId, CreatePaymentRequest createPayment, CancellationToken cancellationToken = default);
}
public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IStripePaymentIntentService _stripePaymentIntentService;
    private readonly IUnitOfWork _unitOfWork;


    public OrderService(
        ILogger<OrderService> logger, 
        IOrderRepository orderRepository,
        IProductRepository productRepository, 
        IOrderProductRepository orderProductRepository, 
        IStripePaymentIntentService stripePaymentIntentService, 
        IUnitOfWork unitOfWork,
        IInventoryRepository inventoryRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _stripePaymentIntentService = stripePaymentIntentService;
        _unitOfWork = unitOfWork;
        _inventoryRepository = inventoryRepository;
    }
    
    public async Task<OrderSummaryResponse> GetByIdAsync(long orderId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting order with id: {orderId}", orderId);
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if(order is null) throw new NotFoundException($"Order {orderId} not found");
        return order.MapToOrderSummary();
    }

    public async Task<OrderSummaryResponse> GetOrderByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting order with payment intent id: {stripePaymentIntentId}", stripePaymentIntentId);
        var order = await _orderRepository.GetByPaymentIntentIdAsync(stripePaymentIntentId, cancellationToken);
        if(order is null) throw new NotFoundException($"Order with payment intent {stripePaymentIntentId} not found");
        return order.MapToOrderSummary();
    }



    public async Task<List<OrderSummaryResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all orders");
        var orders = await _orderRepository.GetAllAsync(queryOptions, cancellationToken);
        return orders.MapToOrderSummaries();
    }



    public async Task<List<OrderSummaryResponse>> GetAllByUserIdAsync(string userId, QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all orders for user with id: {userId}", userId);
        var orders = await _orderRepository.GetAllByUserIdAsync(userId, queryOptions, cancellationToken);
        return orders.MapToOrderSummaries();
    }

    public async Task<OrderSummaryResponse> CreateAsync(CreateOrderRequest createOrderRequest,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating order");
        //first we retrieve the products linked to the order to be created
        var products =
            await _productRepository.GetByIdsAsync(createOrderRequest.Products.Select(p => p.ProductId).ToList(),
                cancellationToken);
        
        //we calculate the total amount based on the products prices and quantities, not written in the request
        var totalAmount = products.Sum(p =>
            p.Price * createOrderRequest.Products.First(op => op.ProductId == p.Id).Quantity);
        
        //we prepare the order data for insertion into the SQL database
        var createOrderSqlRequest = createOrderRequest.MapToCreateOrderRequest(totalAmount);
        createOrderSqlRequest.PaymentStatus = "draft";

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            //we create the order in the SQL database
            var createdOrderId = await _orderRepository.CreateAsync(createOrderSqlRequest, cancellationToken);

            //we then use the id of the created order to create the corresponding links between the order and the products
            var createOrderProductsSqlRequest =
                createOrderRequest.Products.MapToCreateOrderProductRequests(products, createdOrderId);

            //we create the corresponding links between the order and the products in the SQL database
            await _orderProductRepository.CreateManyAsync(createOrderProductsSqlRequest, cancellationToken);

            //we map the created order products to their corresponding products and the order
            var orderResponse = await _orderRepository.GetByIdAsync(createdOrderId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Order created successfully with id: {orderId}", createdOrderId);
            return orderResponse.MapToOrderSummary();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create order: {message}", ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task UpdateOrderPaymentStatus(UpdateOrderSQLRequest updateOrderRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating order payment status to {paymentStatus} for order with id: {orderId}", updateOrderRequest.PaymentStatus, updateOrderRequest.Id);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _orderRepository.UpdateAsync(updateOrderRequest, cancellationToken);
            if (updateOrderRequest.PaymentStatus == "canceled")
            {
                var order = await _orderRepository.GetByIdAsync(updateOrderRequest.Id, cancellationToken);
                if (order is null) throw new NotFoundException($"Order {updateOrderRequest.Id} not found.");
                await RemoveProducts(order, new List<FullProductSQLResponse>(), cancellationToken);
                await DeleteAsync(order.Id, cancellationToken); 
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Order payment status updated successfully for order with id: {orderId}",
                updateOrderRequest.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update order payment status: {message}", ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<OrderSummaryResponse> UpdateAsync(long orderId, UpdateOrderRequest updateOrderRequest,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating order with id: {orderId}", orderId);
        //first we retrieve the order
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null) throw new NotFoundException($"Order {orderId} not found");
        if (order.PaymentStatus is "succeeded") throw new BadRequestException("Cannot update an order with a succeeded payment status");
        
        //then we retrieve the products linked to the order request to ensure the prices are up to date
        var newProducts =
            await _productRepository.GetByIdsAsync(updateOrderRequest.Products.Select(p => p.ProductId).ToList(),
                cancellationToken);
        
        //we calculate the new total amount based on the updated products prices and quantities
        var totalAmount = newProducts.Sum(p =>
            p.Price * updateOrderRequest.Products.First(op => op.ProductId == p.Id).Quantity);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            //we update the order payment intent with the new total amount
            await UpdateOrderPaymentIntent(cancellationToken, totalAmount, order);

            //we remove any products that have been removed from the order request from the SQL database
            await RemoveProducts(order, newProducts, cancellationToken);

            //we update the link between the order and the products with the new prices and quantities
            var updateOrderProductsSqlRequest =
                updateOrderRequest.Products.MapToUpdateOrderProductRequests(newProducts, orderId);
            await _orderProductRepository.UpdateManyAsync(updateOrderProductsSqlRequest,
                cancellationToken);

            //then we update the order itself with the new total amount that we calculated earlier
            var updateOrderSqlRequest = updateOrderRequest.MapToUpdateOrderRequest(orderId, totalAmount);
            var affectedOrderRows = await _orderRepository.UpdateAsync(updateOrderSqlRequest, cancellationToken);
            if (affectedOrderRows == 0) throw new InvalidOperationException("Update failed");
            var updatedRow = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Order updated successfully with id: {orderId}", orderId);
            return updatedRow.MapToOrderSummary();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update order: {message}", ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteAsync(long orderId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting order: {orderId}", orderId);
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _orderRepository.DeleteAsync(orderId, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException("Delete failed");
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Order deleted successfully with id: {orderId}", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error deleting order {orderId} : {message}", orderId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<PaymentIntent> CreatePaymentAsync(long orderId, string userId, CreatePaymentRequest createPaymentRequest,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating payment intent for order {orderId}", orderId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var order = await GetByIdAsync(orderId, cancellationToken);
            if (!order.OrderItems.Any()) throw new BadRequestException($"Order {orderId} doesnt have any items.");
            if (order.PaymentStatus == "succeeded") throw new BadRequestException($"Cannot create payment intent for an order with a succeeded payment status");
            
            _logger.LogInformation("Decrementing stock for order {orderId}", orderId);
            foreach (var orderProduct in order.OrderItems)
            {
                await DecrementAsync((int)orderProduct.Quantity, (long)orderProduct.ProductId,
                    cancellationToken);
            }
            _logger.LogInformation("Stock decremented for order {orderId}", orderId);

            _logger.LogInformation("Creating payment intent for order {orderId}", orderId);
            var request = createPaymentRequest.ToStripePaymentIntentOptions(userId, (long)order.TotalAmount);
            var paymentIntent = await _stripePaymentIntentService.CreateAsync(request, null, cancellationToken);
            _logger.LogInformation("Payment intent {paymentId} created for order {orderId}", paymentIntent.Id, orderId);
            
            var updateOrderSqlRequest = new UpdateOrderSQLRequest() { Id = orderId, StripePaymentIntentId = paymentIntent.Id, PaymentStatus = "created"};
            var affectedRow = await _orderRepository.UpdateAsync(updateOrderSqlRequest, cancellationToken);
            if (affectedRow == 0) throw new NotFoundException($"Cannot create payment intent for order {orderId}, not found.");
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Payment intent created successfully for order {orderId}", orderId);
            return paymentIntent;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating payment intent for order {orderId}", orderId);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task RemoveProducts(OrderSummarySQLResponse order, List<FullProductSQLResponse> newProducts, CancellationToken cancellationToken = default)
    {
        //then we retrieve the old products linked to the order to see if some products have been removed
        var oldProductOrderList = await _orderProductRepository.GetAllByOrderIdAsync(order.Id, cancellationToken);
        var oldProducts =
            await _productRepository.GetByIdsAsync(oldProductOrderList.Select(op => op.ProductId).ToList(),
                cancellationToken);
        
        var productsToRemove = oldProducts.Where(op => newProducts.All(np => np.Id != op.Id)).ToList();
        var productsLinksToRemove = oldProductOrderList.Where(op => productsToRemove.Any(np => np.Id == op.ProductId)).ToList();

        foreach (var productLink in productsLinksToRemove)
        {
            await _orderProductRepository.DeleteAsync(productLink.Id, cancellationToken);
            await IncreamentAsync(productLink.Quantity, productLink.Id, cancellationToken);
        }

        foreach (var product in productsToRemove)
        {
            await _orderRepository.DeleteAsync(product.Id, cancellationToken);
        }
    }

    private async Task UpdateOrderPaymentIntent(CancellationToken cancellationToken, double totalAmount,
        OrderSummarySQLResponse order)
    {
        //if the new total amount is different from the current one, we update the payment intent amount
        if (Math.Abs(totalAmount - order.TotalAmount) > 1 && !string.IsNullOrEmpty(order.StripePaymentIntentId))
        {
            await _stripePaymentIntentService.UpdateAsync(
                order.StripePaymentIntentId, 
                new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(totalAmount * 100), 
                    Description = $"Updated order {order.Id}" 
                }, 
                null, 
                cancellationToken);
        }
    }
    
    private async Task DecrementAsync(int substractedQuantity, long productId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Decreasing inventory quantity by {substractedQuantity} for product id: {productId}", substractedQuantity, productId);
        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        if (inventory.Quantity < substractedQuantity) throw new BadRequestException($"Substracted Quantity ({substractedQuantity}) is superior to the actual quantity ({inventory.Quantity})");
        var newQuantity = inventory.Quantity - substractedQuantity;
        var sqlRequest = new UpdateInventorySQLRequest()
        {
            Id = inventory.Id,
            Quantity = newQuantity,
        };

        var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
        if (affectedRows == 0) throw new NotFoundException($"Inventory for the following product: {productId} not found");
        var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
        _logger.LogInformation("Inventory quantity decreased by {substractedQuantity} for product id: {productId} successfully.", substractedQuantity, productId);
    }
    
    private async Task IncreamentAsync(int addedQuantity,long productId,
        CancellationToken cancellationToken = default)
    {   
        _logger.LogInformation("Increasing inventory quantity by {addedQuantity} for product id: {productId}", addedQuantity, productId);
        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        var newQuantity = inventory.Quantity + addedQuantity;
        var sqlRequest = new UpdateInventorySQLRequest()
        {
            Id = inventory.Id,
            Quantity = newQuantity,
        };
        
        var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
        if (affectedRows == 0) throw new NotFoundException($"Inventory for the following product: {productId} not found");
        var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("Inventory quantity increased by {addedQuantity} for product id: {productId} successfully.", addedQuantity, productId);
    }
}