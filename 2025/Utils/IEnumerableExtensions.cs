namespace _2025.Utils;

public static class IEnumerableExtensions
{
    public static IEnumerable<(T item, int index)> Enumerate<T>(this IEnumerable<T>? self)       
        => self?.Select((item, index) => (item, index)) ?? [];

    
    public static bool TryGetValue<T>(this List<T[]> input, ValueTuple<int, int> position, out T? value)
    {
        if(IsOob(input, position))
        {
            value = default;
            return false;
        }

        value = input[position.Item1][position.Item2];
        return true;
    }
    
    public static bool IsOob<T>(this List<T[]> input, ValueTuple<int, int> position)
    {
        return (position.Item1 < 0 || position.Item1 >= input.Count || position.Item2 < 0 ||
                position.Item2 >= input[position.Item1].Length);
    }
}