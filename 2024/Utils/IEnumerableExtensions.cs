namespace _2024.Utils;

public static class IEnumerableExtensions
{
    public static IEnumerable<(T item, int index)> Enumerate<T>(this IEnumerable<T>? self)       
        => self?.Select((item, index) => (item, index)) ?? [];

}