namespace Tdev702.Contracts.SQL.Request.OrderProduct;

public class UpdateOrderProductSQLRequest
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public required int Quantity { get; set; }
    public required double Subtotal { get; set; }
}