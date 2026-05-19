namespace InterviewPrep.Shared.Domain;

public class Order(string orderId, string customerName)
{
    public string OrderId { get; } = orderId;
    public string CustomerName { get; } = customerName;
    public List<OrderItem> Items { get; } = [];
    public required string ShippingAddress { get; init; }

    public decimal Total => Items.Sum(x => x.TotalPrice);

    public void AddItem(OrderItem item) => Items.Add(item);
}
