using System.Collections.Frozen;

namespace InterviewPrep.Console.Services;

public sealed class InventoryService
{
    // FrozenDictionary is a notable .NET 8 optimization for read-heavy workloads.
    private readonly FrozenDictionary<string, int> _stockBySku;

    public InventoryService(Dictionary<string, int> seed)
    {
        _stockBySku = seed.ToFrozenDictionary();
    }

    public bool IsInStock(string sku, int quantity) =>
        _stockBySku.TryGetValue(sku, out var stock) && stock >= quantity;
}
