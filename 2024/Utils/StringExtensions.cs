using System.ComponentModel;

namespace _2024.Utils;

public static class StringExtensions
{
    /// <summary>
    /// Safely get the value at <paramref name="position"/>
    /// </summary>
    /// <param name="input">The string array</param>
    /// <param name="position">The row/column of the position to get</param>
    /// <param name="value">Contains the value at <paramref name="position"/></param>
    /// <returns>true if the position is in bounds, else false</returns>
    public static bool TryGetValue(this string[] input, ValueTuple<int, int> position, out char? value)
    {
        if(IsOob(input, position))
        {
            value = default;
            return false;
        }

        value = input[position.Item1][position.Item2];
        return true;
    }
    

    

    /// <summary>
    /// Does out of bounds checks on the given <paramref name="input"/>. 
    /// </summary>
    /// <param name="input">The input in form of an array of strings</param>
    /// <param name="position">The row/col indices to check</param>
    /// <returns>false if the position is bounds, else true</returns>
    public static bool IsOob(this string[] input, ValueTuple<int, int> position)
    {
        return (position.Item1 < 0 || position.Item1 >= input.Length || position.Item2 < 0 ||
                position.Item2 >= input[position.Item1].Length);
    }
    

}