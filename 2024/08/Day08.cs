using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._08;



public class Day08 : Base
{
    public Day08(bool example) : base(example) 
    {
        Day = "8";
    }

    private int Rows { get; set; }
    private int Columns { get; set; }

    private void AddAntinodes(ValueTuple<int, int> direction, ValueTuple<int, int> start,
        List<ValueTuple<int, int>> antinodes, int stage)
    {
        start = (start.Item1 + direction.Item1, start.Item2 + direction.Item2);
        while (start is { Item1: >= 0, Item2: >= 0 } && start.Item1 < Rows && start.Item2 < Columns)
        {
            antinodes.Add(start);
            if (stage == 1)
            {
                break;
            }
            start = (start.Item1 + direction.Item1, start.Item2 + direction.Item2);
        }
    }

    private int SolvePuzzle(string[] input, int stage)
    {
        Dictionary<char, List<ValueTuple<int, int>>> antennas = [];
        List<ValueTuple<int, int>> antinodes = [];
        Rows = input.Length;
        Columns = input[0].Length;
        foreach ((string line, int row) in input.Enumerate())
        {
            foreach ((char position, int col) in line.Enumerate())
            {
                if (position == '.')
                {
                    continue;
                }
                if (!antennas.TryGetValue(position, out List<(int, int)>? value))
                {
                    value = ([]);
                    antennas[position] = value;
                }
                value.Add((row, col));
                if (stage == 2)
                {
                    antinodes.Add((row, col));
                }
            }
        }
        foreach (char key in antennas.Keys)
        {
            for(int i = 0; i < antennas[key].Count; i++)
            {
                for (int j = i+1; j < antennas[key].Count; j++)
                {
                    ValueTuple<int, int> direction = (antennas[key][i].Item1 - antennas[key][j].Item1,
                        antennas[key][i].Item2 - antennas[key][j].Item2);
                    ValueTuple<int, int> firstAntiNode = (antennas[key][i].Item1, antennas[key][i].Item2);
                    ValueTuple<int, int> secondAntiNode = (antennas[key][j].Item1, antennas[key][j].Item2);
                    AddAntinodes(direction, firstAntiNode, antinodes, stage);
                    direction = (direction.Item1 * -1, direction.Item2 * -1);
                    AddAntinodes(direction, secondAntiNode, antinodes, stage);
                }
            }
        }

        return antinodes.Distinct().Count();
    }
    
    public override object PartOne()
    {
        string[] input = ReadInput();
        return SolvePuzzle(input, 1);
    }
    
    public override object PartTwo()
    {
        string[] input = ReadInput();
        return SolvePuzzle(input, 2);
    }
}