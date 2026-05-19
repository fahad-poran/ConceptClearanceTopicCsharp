using System.Collections.Frozen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("FrontendDev");

// Minimal API endpoint used by the browser app for a real-time order calculation demo.
app.MapPost("/api/order/calculate", (OrderRequest request) =>
{
    FrozenDictionary<string, int> inventory = new Dictionary<string, int>
    {
        ["LAPTOP-001"] = 10,
        ["MOUSE-010"] = 50,
        ["BAG-200"] = 20
    }.ToFrozenDictionary();

    var acceptedItems = new List<OrderItem>();

    foreach (var item in request.Items)
    {
        if (inventory.TryGetValue(item.Sku, out var stock) && stock >= item.Quantity)
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

    var response = new OrderCalculationResponse(
        RequestId: $"REQ-{DateTime.UtcNow:yyyyMMddHHmmss}",
        AcceptedItems: acceptedItems,
        GrossTotal: gross,
        DiscountAmount: discountAmount,
        DiscountReason: reason,
        FinalTotal: gross - discountAmount,
        GeneratedAtUtc: DateTime.UtcNow
    );

    return Results.Ok(response);
});

app.MapFallbackToFile("index.html");
app.Run();

public sealed record OrderRequest(string CustomerName, IReadOnlyList<OrderItem> Items);
public readonly record struct OrderItem(string Sku, int Quantity, decimal UnitPrice);

public sealed record OrderCalculationResponse(
    string RequestId,
    IReadOnlyList<OrderItem> AcceptedItems,
    decimal GrossTotal,
    decimal DiscountAmount,
    string DiscountReason,
    decimal FinalTotal,
    DateTime GeneratedAtUtc
);
