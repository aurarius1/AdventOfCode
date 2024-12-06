using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._06;



public class Day06 : Base
{
    public Day06()
    {
        Day = "6";
    }

    private static readonly ValueTuple<int, int>[] Directions =
    [
        (-1, 0), // up start
        (0, 1), // right
        (1, 0), // down
        (0, -1) // left
    ];
    
    private string Facility { get; set; }
    private int Rows { get; set; }
    private int Columns { get; set; }


    
    private bool WalkRoute(
        ValueTuple<int, int> guard, 
        int stage,
        ValueTuple<int, int> placedObstacle,
        out HashSet<ValueTuple<ValueTuple<int, int>, int>> visited)
    {
        int direction = 0; 
        visited = [];
        
        HashSet<ValueTuple<int, int>> visitedObstacles = [];
        while (true)
        {
            // add -1 as direction if stage is 1
            if (!visited.Add((guard, stage == 2 ? direction : -1)) && stage == 2)
            {
                // loop 
                return true;
            }
            int newRow = guard.Item1 + Directions[direction].Item1, newCol = guard.Item2 + Directions[direction].Item2;
            if (newRow < 0 || newRow >= Rows || newCol < 0 || newCol >= Columns)
            {
                break;
            }
            if (Facility[newRow * Rows + newCol] == '#')
            {
                direction = (direction + 1) % 4;
                continue;
            }

            // default value for PlacedObstacle is (-1, -1), stage 1 should not be affected by that
            if (stage == 2 && newRow == placedObstacle.Item1 && newCol == placedObstacle.Item2)
            {
                direction = (direction + 1) % 4;
                continue;
            }
            guard = (newRow, newCol);
        }
        return false;
    }

    // Runs in about .4s 
    private int Part2Parallelized(ValueTuple<int, int> guard, HashSet<ValueTuple<ValueTuple<int, int>, int>> visited)
    {
        ParallelOptions options = new() { };
        var concurrentObstacles = new ConcurrentBag<(int, int)>();
        Parallel.ForEach(visited, options, spot =>
        {
            (int x, int y) = spot.Item1;
            if (WalkRoute(guard, 2, (x, y), out var _))
            {
                concurrentObstacles.Add((x, y));
            }
        });
        HashSet<ValueTuple<int, int>> obstacles = [];
        foreach (var obstacle in concurrentObstacles)
        {
            obstacles.Add(obstacle);
        }
        return obstacles.Count;
    }
    
    
    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);

        Facility = string.Join("", input);
        Rows = input.Length; 
        Columns = input[0].Length;

        ValueTuple<int, int> guard = (0, 0);
        foreach ((char spot, int idx) in Facility.Enumerate())
        {
            int row = idx / Rows, col = idx % Columns;
            if (spot != '^')
            {
                continue;
            }
            guard = (row, col);
            break;
        }
        
        WalkRoute(guard, 1, (-1, -1), out var visited);
        return visited.Count;
    }
    
    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);

        Facility = string.Join("", input);
        Rows = input.Length; 
        Columns = input[0].Length;
        
        ValueTuple<int, int> guard = (0, 0);
        foreach ((char spot, int idx) in Facility.Enumerate())
        {
            int row = idx / Rows, col = idx % Rows;
            if (spot != '^')
            {
                continue;
            }
            guard = (row, col);
            break;
        }
        WalkRoute(guard, 1, (-1, -1), out var visited);
        HashSet<ValueTuple<int, int>> obstacles = [];
        foreach ((ValueTuple<int, int> spot, int _) in visited)
        {
            if (WalkRoute(guard, 2, spot, out var _))
            {
                obstacles.Add(spot);
            }
        }
        return obstacles.Count;
    }
}