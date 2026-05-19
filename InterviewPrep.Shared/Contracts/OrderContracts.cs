namespace InterviewPrep.Shared.Contracts;

public sealed record OrderRequest(string CustomerName, IReadOnlyList<OrderItem> Items);

public readonly record struct OrderItem(string Sku, int Quantity, decimal UnitPrice)
{
    public decimal TotalPrice => Quantity * UnitPrice;
}

public sealed record OrderCalculationResponse(
    string RequestId,
    IReadOnlyList<OrderItem> AcceptedItems,
    decimal GrossTotal,
    decimal DiscountAmount,
    string DiscountReason,
    decimal FinalTotal,
    DateTime GeneratedAtUtc
);
