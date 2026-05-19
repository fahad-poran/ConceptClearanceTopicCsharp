using System.Collections.Frozen;

namespace InterviewPrep.Shared.Services;

public sealed class InventoryService
{
    private readonly FrozenDictionary<string, int> _stockBySku;

    public InventoryService(IEnumerable<KeyValuePair<string, int>> seed)
    {
        _stockBySku = seed.ToFrozenDictionary();
    }

    public bool IsInStock(string sku, int quantity) =>
        _stockBySku.TryGetValue(sku, out var stock) && stock >= quantity;

    public IReadOnlyDictionary<string, int> Snapshot() => _stockBySku;
}
