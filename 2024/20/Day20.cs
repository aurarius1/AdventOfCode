
using System.Collections.Immutable;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._20;



public class Day20 : Base
{
    private Dictionary<int, int> Cheats { get; } = new();
    private ValueTuple<int, int> Start { get; set; }
    private ValueTuple<int, int> End { get; set; }
    private List<char[]> Map { get; set; }
    private ValueTuple<int, int>[] Directions { get; } = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public Day20(bool example) : base(example)
    {
        Day = "20";
        ParseInput();
    }

    private void ParseInput()
    {
        string[] input = ReadInput();
        Map = new List<char[]>(input.Length);
        foreach ((string line, int idx) in input.Enumerate())
        {
            Map.Add(line.ToCharArray());
            if (line.Contains('S'))
            {
                Start = (idx, line.IndexOf('S'));
            }

            if (line.Contains('E'))
            {
                End = (idx, line.IndexOf('E'));
            }
        }
        Map[Start.Item1][Start.Item2] = '.';
        Map[End.Item1][End.Item2] = '.';
    }

    private int FindPath(out SortedDictionary<ValueTuple<int, int>, int> visited)
    {
        PriorityQueue<ValueTuple<int, int>, int> path = new();
        visited = [];
        path.Enqueue(Start, 0);
        while (path.TryDequeue(out ValueTuple<int, int> pos, out int stepsTaken))
        {
            if (pos == End)
            {
                visited.Add(pos, stepsTaken);
                return stepsTaken;
            }
            
            if (!visited.TryAdd(pos, stepsTaken))
            {
                continue;
            }

            foreach (ValueTuple<int, int> dir in Directions)
            {
                ValueTuple<int, int> newPos = (pos.Item1+dir.Item1, pos.Item2+dir.Item2);
                if(!Map.TryGetValue(newPos, out char value) || value == '#')
                {
                    continue;
                }
                path.Enqueue(newPos, stepsTaken+1);
            }
        }
        return 0;
    }
    
    private int FindDistinctCheats(int stage)
    {
        int pathWithoutCheats = FindPath(out SortedDictionary<ValueTuple<int, int>, int> path);
        int totalNumberOfCheats = 0;
        // one could also check all positions where the difference between pos and cheatPos would be 
        // >= 100 and the distance between the two positions would be <= 20 (for stage 2)
        int cap = (stage == 1 ? 2 : 20);
        foreach ((ValueTuple<int, int> pos, int time) in path)
        {
            for (int row = -cap ; row < cap+1; row++)
            {
                int rowSteps = Math.Abs(row);
                for (int col = -(cap-rowSteps); col < (cap-rowSteps)+1; col++)
                {
                    int cheatCost = Math.Abs(col) + rowSteps;
                    // check if distance is bigger than allowed distance
                    ValueTuple<int, int> newPos = (pos.Item1+row, pos.Item2+col);
                    if (!path.TryGetValue(newPos, out int value))
                    {
                        continue;
                    }
                    
                    int saving = pathWithoutCheats - (time + pathWithoutCheats - value + cheatCost);
                    //saving = value - time + cheatCost;
                    
                    if (saving >= (Example ? 1 : 100))
                    {
                        totalNumberOfCheats++;
                    }
                }
            }
        }
        return totalNumberOfCheats;
    }
    
    private int FindDistinctCheats2(int stage)
    {
        int pathWithoutCheats = FindPath(out SortedDictionary<ValueTuple<int, int>, int> path);
        int totalNumberOfCheats = 0;
        foreach ((ValueTuple<int, int> pos, int time) in path)
        {
            //List<ValueTuple<int, int>> cheats = path.Keys.Where(x => path[x])
        }
        return totalNumberOfCheats;
    }
    
    public override object PartOne()
    {
        return FindDistinctCheats(1);
    }
    
    public override object PartTwo()
    {
        return FindDistinctCheats2(2);
    }

    public override void Reset()
    {
        // removing this line makes part 2 use the cache build from part 1
        // effectively making it run instantly 
        Cheats.Clear();
    }
}

