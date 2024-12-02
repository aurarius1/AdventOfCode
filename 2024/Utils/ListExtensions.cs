namespace _2024.Utils;

public static class ListExtensions
{
    public static int IndexOfMin(this List<int> list)
    {
        if (list.Count == 0)
        {
            return -1;
        }
        int min = list[0];
        int minIndex = 0;

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

}