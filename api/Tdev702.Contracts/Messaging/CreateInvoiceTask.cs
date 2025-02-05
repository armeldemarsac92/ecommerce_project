using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Messaging;

public class CreateInvoiceTask
{
    public required OrderSummaryResponse order { get; init; }
}