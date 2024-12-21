
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._18;


public class Day18 : Base
{
    public Day18(bool example) : base(example)
    {
        Day = "18";
    }


    private int GetShortestPath(List<char[]> memoryArea, ImmutableSortedSet<ValueTuple<int, int>> corruptingBytes, ValueTuple<int, int> start, ValueTuple<int, int> end)
    {
        PriorityQueue<ValueTuple<int, int>, int> path = new();
        HashSet<ValueTuple<int, int>> visited = [];
        path.Enqueue(start, 0);
        while (path.TryDequeue(out ValueTuple<int, int> pos, out int stepsTaken))
        {
            if (pos.Item1+1 == end.Item1 && pos.Item2+1 == end.Item2)
            {
                return stepsTaken;
            }
            
            if (!visited.Add(pos))
            {
                continue;
            }

            foreach (ValueTuple<int, int> dir in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
            {
                ValueTuple<int, int> newPos = (pos.Item1+dir.Item1, pos.Item2+dir.Item2);
                if(!memoryArea.TryGetValue(newPos, out char _) || corruptingBytes.Contains(newPos))
                {
                    continue;
                }
                path.Enqueue(newPos, stepsTaken+1);
            }
        }

        return -1;
    }
    
    private string FindBlockingPosition(List<char[]> memoryArea, List<ValueTuple<int, int>> corruptingBytes, ValueTuple<int, int> start, ValueTuple<int, int> end)
    {
        int low = 0; 
        int high = corruptingBytes.Count - 1;
        int mid = 0;
        while (low <= high)
        {
            mid = low + (high - low) / 2;
            int shortestPathMid =
                GetShortestPath(memoryArea, corruptingBytes[..(mid+1)].ToImmutableSortedSet(), start, end);
            int shortestPathOnePrior = GetShortestPath(memoryArea, corruptingBytes[..mid].ToImmutableSortedSet(),
                start, end);

            if (shortestPathOnePrior != -1 && shortestPathMid == -1)
            {
                break;
            }

            if (shortestPathMid != -1)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }
        return $"{corruptingBytes[mid].Item2},{corruptingBytes[mid].Item1}";
    }

    
    public override object PartOne()
    {
        string[] input = ReadInput();
        ValueTuple<int, int> start = (0, 0);
        ValueTuple<int, int> end = Example ? (7, 7) : (71, 71);
        List<char[]> memoryArea = new(end.Item1);
        SortedSet<ValueTuple<int, int>> corruptingBytes = [];
        for (int i = 0; i < end.Item1; i++)
        {
            memoryArea.Add(new char[end.Item2]);
        }
        
        foreach ((string bytePos, int idx) in input.Enumerate())
        {
            if (idx == (Example ? 12 : 1024))
            {
                break;
            }
            string[] split = bytePos.Split(',');
            ValueTuple<int, int> currByte = (int.Parse(split[1]), int.Parse(split[0]));
            corruptingBytes.Add(currByte);
        }
        return GetShortestPath(memoryArea, corruptingBytes.ToImmutableSortedSet(), start, end);
    }
    
    public override object PartTwo()
    {
        string[] input = ReadInput();
        ValueTuple<int, int> start = (0, 0);
        ValueTuple<int, int> end = Example ? (7, 7) : (71, 71);
        List<char[]> memoryArea = new(end.Item1);
        for (int i = 0; i < end.Item1; i++)
        {
            memoryArea.Add(new char[end.Item2]);
        }
        List<ValueTuple<int, int>> corruptingBytes = [];
        corruptingBytes.AddRange(input.Select(bytePos => bytePos.Split(',')).Select(split => (int.Parse(split[1]), int.Parse(split[0]))));

        return FindBlockingPosition(memoryArea, corruptingBytes, start, end);
    }

    public override void Reset()
    {

    }
}

