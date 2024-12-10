using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._10_refactored;



public class Day10Refactored : Base
{
    public Day10Refactored()
    {
        Day = "10.1";
    }
    
    private int[][] Map { get; set; }
    private HashSet<ValueTuple<int, int>> _visited = [];
    private readonly ValueTuple<int, int>[] Directions =
    [
        (0, 1),
        (0, -1),
        (1, 0),
        (-1, 0)
    ];
    
    private void PrintMap(){
        foreach ((int[] line, int row) in Map.Enumerate())
        {
            foreach ((int height, int col) in line.Enumerate())
            {
                Console.Write(height);
            }
            Console.WriteLine();
        }

        Console.WriteLine();}
    
    private int SearchPaths(ValueTuple<int, int> position, int stage = 2)
    {
        if (!_visited.Add(position))
        {
            if (stage == 2)
            {
                _visited.Remove(position);
            }
            return 0;
        }
    
        if (Map[position.Item1][position.Item2] == 9)
        {
            if (stage == 2)
            {
                _visited.Remove(position);
            }
            return 1;
        }
        
        int paths = 0;
        foreach ((int row, int col) in Directions)
        {
            ValueTuple<int, int> newPosition = (position.Item1+row, position.Item2+col);

            if (newPosition.Item1 < 0 || newPosition.Item1 >= Map.Length || newPosition.Item2 < 0 ||
                newPosition.Item2 >= Map[newPosition.Item1].Length)
            {
                continue;
            }

            if (Map[position.Item1][position.Item2] + 1 != Map[newPosition.Item1][newPosition.Item2])
            {
                continue;
            }
            
            paths += SearchPaths(newPosition, stage);
            
        }

        if (stage == 2)
        {
            _visited.Remove(position);
        }
        return paths;
    }

    private int SolvePuzzle(bool example, int stage)
    {
        string[] input = ReadInput(example);
        Map = new int[input.Length][];
        for (int row = 0; row < input.Length; row++)
        {
            Map[row] = input[row].ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
        }

        int score = 0;
        foreach ((int[] row, int rowIdx) in Map.Enumerate())
        {
            foreach ((int height, int col) in row.Enumerate())
            {
                if (height != 0)
                {
                    continue;
                }

                _visited.Clear();
                score += SearchPaths((rowIdx, col), stage);
            }
        }

        return score;
    }
    
    public override object PartOne(bool example)
    {
        return SolvePuzzle(example, 1);
    }
    
    public override object PartTwo(bool example)
    {
        return SolvePuzzle(example, 2);
    }
}