using InterviewPrep.Shared.Contracts;

namespace InterviewPrep.Shared.Services;

public sealed class OrderCalculationService
{
    private readonly InventoryService _inventory;

    public OrderCalculationService(InventoryService inventory)
    {
        _inventory = inventory;
    }

    public OrderCalculationResponse Calculate(OrderRequest request, DateTime utcNow)
    {
        var acceptedItems = new List<OrderItem>();

        foreach (var item in request.Items)
        {
            if (_inventory.IsInStock(item.Sku, item.Quantity))
            {
                acceptedItems.Add(item);
            }
        }

        var gross = acceptedItems.Sum(x => x.Quantity * x.UnitPrice);
        var (discountAmount, reason) = gross switch
        {
            >= 1000m => (gross * 0.10m, "10% for orders >= 1000"),
            _ when acceptedItems.Count >= 5 => (gross * 0.05m, "5% for 5+ accepted items"),
            _ => (0m, "No discount")
        };

        return new OrderCalculationResponse(
            RequestId: $"REQ-{utcNow:yyyyMMddHHmmss}",
            AcceptedItems: acceptedItems,
            GrossTotal: gross,
            DiscountAmount: discountAmount,
            DiscountReason: reason,
            FinalTotal: gross - discountAmount,
            GeneratedAtUtc: utcNow
        );
    }
}
