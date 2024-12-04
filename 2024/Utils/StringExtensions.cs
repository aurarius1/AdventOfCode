namespace _2024.Utils;

public static class StringExtensions
{
    public static bool TryGetValue(this string str, int key, out char? value)
    {
        try
        {
            value = str[key];
        }
        catch (IndexOutOfRangeException ex)
        {
            value = null;
            return false;
        }

        return true;
    }
}