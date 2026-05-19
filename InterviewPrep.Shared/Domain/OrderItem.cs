namespace InterviewPrep.Shared.Domain;

public readonly record struct OrderItem(string Sku, int Quantity, decimal UnitPrice)
{
    public decimal TotalPrice => Quantity * UnitPrice;
}
