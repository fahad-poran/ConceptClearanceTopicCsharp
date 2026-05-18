namespace InterviewPrep.Console.Domain;

// readonly record struct: value-type semantic + immutable data model.
public readonly record struct OrderItem(string Sku, int Quantity, decimal UnitPrice)
{
    public decimal TotalPrice => Quantity * UnitPrice;
}
