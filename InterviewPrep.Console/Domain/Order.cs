namespace InterviewPrep.Console.Domain;

// Primary constructor (C# 12) used here for concise immutable setup.
public class Order(string orderId, string customerName)
{
    public string OrderId { get; } = orderId;
    public string CustomerName { get; } = customerName;
    public List<OrderItem> Items { get; } = [];

    // required + init are often asked in interviews for DTO correctness.
    public required string ShippingAddress { get; init; }

    public decimal Total => Items.Sum(x => x.TotalPrice);

    public void AddItem(OrderItem item) => Items.Add(item);
}
