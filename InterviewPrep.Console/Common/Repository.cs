namespace InterviewPrep.Console.Common;

// Generic repository example for interview discussion.
public interface IRepository<T>
{
    void Add(T item);
    IReadOnlyList<T> GetAll();
}

public sealed class InMemoryRepository<T> : IRepository<T>
{
    private readonly List<T> _items = [];

    public void Add(T item) => _items.Add(item);

    public IReadOnlyList<T> GetAll() => _items;
}

public static class GenericUtility
{
    // Generic method with constraints is a frequent question.
    public static T MaxByValue<T>(T left, T right) where T : IComparable<T>
        => left.CompareTo(right) >= 0 ? left : right;
}
