namespace Tdev702.Contracts.SQL.Request.OrderProduct;

public class CreateOrderProductSQLRequest
{
    public required long ProductId { get; set; }
    public required long OrderId { get; set; }
    public required int Quantity { get; set; }
    public required double UnitPrice { get; set; }
    public required double Subtotal { get; set; }
}