using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Response;

public class OrderProductSQLResponse
{
    [Column("id")]
    public required long Id { get; set; }

    [Column("product_id")]
    public required long ProductId { get; set; }

    [Column("order_id")]
    public required long OrderId { get; set; }

    [Column("quantity")]
    public required int Quantity { get; set; }
    
    [Column("unit_price")]
    public required double UnitPrice { get; set; }

    [Column("subtotal")]
    public required double Subtotal { get; set; }
}