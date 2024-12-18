
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._16;

public struct Box
{   
    
    public ValueTuple<int, int, char> Open { get; set; }
    public ValueTuple<int, int> Close { get; set; }
}


public class Day16 : Base
{
    public Day16()
    {
        Day = "16";
    }

    private readonly ValueTuple<int, int>[] _directions =
    [
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0)
    ];

    private int FindLowestScore(string[] input)
    {
        ValueTuple<int, int> start = (0,0), end = (0,0);
        foreach ((string line, int row) in input.Enumerate())
        {
            if (line.Contains('E'))
            {
                end = (row, line.IndexOf('E'));
            }

            if (line.Contains('S'))
            {
                start = (row, line.IndexOf('S'));
            }
        }
        
        PriorityQueue<ValueTuple<ValueTuple<int, int>, int>, int> queue = new();
        queue.Enqueue((start, 0), 0 );
        HashSet<ValueTuple<ValueTuple<int, int>, int>> visited = [];
        
        while (queue.TryDequeue(out ValueTuple<ValueTuple<int, int>, int> curr, out int priority))
        {
            if (curr.Item1 == end)
            {
                return priority;
            }
            if (!visited.Add(curr))
            {
                continue;
            }
            
            for (int direction = -1; direction < 2; direction += 2)
            {
                queue.Enqueue((curr.Item1, MathExtensions.Modulo(curr.Item2 + direction, _directions.Length)), priority + 1000);
            }
            // dont turn, move forward to currently facing direction
            ValueTuple<int, int> facing = _directions[curr.Item2];
            ValueTuple<int, int> newPos = (curr.Item1.Item1 + facing.Item1, curr.Item1.Item2 + facing.Item2);
            if (input.TryGetValue(newPos, out char? value) && value != '#')
            {
                queue.Enqueue((newPos, curr.Item2), priority + 1);
            }
        }

        return 0;
    }
    
    private int FindNumberOfSeats(string[] input)
    {
        ValueTuple<int, int> start = (0, 0), end = (0, 0);
        foreach ((string line, int row) in input.Enumerate())
        {
            if (line.Contains('E'))
            {
                end = (row, line.IndexOf('E'));
            }

            if (line.Contains('S'))
            {
                start = (row, line.IndexOf('S'));
            }
        }

        PriorityQueue<ValueTuple<ValueTuple<int, int>, int, HashSet<ValueTuple<int, int>>>, int> queue = new();
        queue.Enqueue((start, 0, []), 0);
        Dictionary<ValueTuple<int, int, int>, int> visited = new();

        int lowestScore = int.MaxValue;
        HashSet<ValueTuple<int, int>> tiles = [];

        while (queue.TryDequeue(out ValueTuple<ValueTuple<int, int>, int, HashSet<ValueTuple<int, int>>> curr, out int priority))
        {
            // skip entirely
            if(priority > lowestScore)
            {
                continue;
            }
            
            if (curr.Item1 == end)
            {
                lowestScore = priority;
                tiles.UnionWith(curr.Item3);
                continue;
            }
            ValueTuple<int, int, int> visitedKey = (curr.Item1.Item1, curr.Item1.Item2, curr.Item2);
            
            // if current field with direction has been seen, skip if seen priority is not equal to current priority 
            // if it is not equal to current priority there is no way, this path would lead to a value where the total 
            // score is optimal, because the remainder of the path has already been seen
            if (visited.TryGetValue(visitedKey, out int i) && i != priority)
            {
                continue;
            }
            // this field, with this direction has been seen with this priority
            visited[visitedKey] = priority;
            HashSet<ValueTuple<int, int>> newHistory = [..curr.Item3, curr.Item1];
            for (int direction = -1; direction < 2; direction++)
            {
                int newDir = MathExtensions.Modulo(curr.Item2 + direction, _directions.Length);
                ValueTuple<int, int> dir = _directions[newDir];
                ValueTuple<int, int> newPos = (curr.Item1.Item1 + dir.Item1, curr.Item1.Item2 + dir.Item2);
                
                if (curr.Item3.Contains(newPos))
                {
                    continue;
                }
                
                if (!input.TryGetValue(newPos, out char? val) || val == '#')
                {
                    continue;
                }

                // do turn and next step at once
                int cost = direction == 0 ? 1 : 1001;
                queue.Enqueue((newPos, newDir, newHistory), priority + cost);
            }
        }

        tiles.Add(end);
        return tiles.Count;
    }
    

    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        return FindLowestScore(input);
    }

    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        return FindNumberOfSeats(input);
    }


    public override void Reset()
    {

    }
}

