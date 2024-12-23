
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._15;

public struct Box
{   
    
    public ValueTuple<int, int, char> Open { get; set; }
    public ValueTuple<int, int> Close { get; set; }
}


public class Day15 : Base
{
    public Day15(bool example) : base(example) 
    {
        Day = "15";
    }
    
    private static void ParseInput(string[] input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, int stage)
    {
        bool readInstructions = false;
        map = [];
        instructions = [];
        robot = (0, 0);
        foreach ((string line, int row) in input.Enumerate())
        {
            if (readInstructions)
            {
                instructions.AddRange(line.ToCharArray());
                continue;
            }

            if (line.Length == 0)
            {
                readInstructions = true;
                continue;
            }

            if (stage == 1)
            {
                map.Add(line.Replace('@', '.').ToCharArray());
                if (line.Contains('@'))
                {
                    robot = (row, line.IndexOf('@'));
                }
                continue;
            }
            
            map.Add(new char[line.Length*2]);
            int idx = 0;
            foreach (char spot in line)
            {
                switch (spot)
                {
                    case '#':
                    case '.':
                        map[^1][idx++] = spot;
                        map[^1][idx++] = spot;
                        break;
                    case 'O':
                        map[^1][idx++] = '[';
                        map[^1][idx++] = ']';
                        break;
                    case '@':
                        robot = (row, idx);
                        map[^1][idx++] = '.';
                        map[^1][idx++] = '.';
                        break;
                }
            }
        }
    }
    
    private static int GetCoordinates(List<char[]> map, char boxCharacter = 'O')
    {
        int gpsCoordinates = 0;
        for (int row = 0; row < map.Count; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] == boxCharacter)
                {
                    gpsCoordinates += (100 * row + col);
                }
            }
        }
        return gpsCoordinates;
    }

    private static int PerformInstructions(List<char[]> map, List<char> instructions, ValueTuple<int, int> robot, int stage)
    {
        foreach(char currInstruction in instructions)
        {
            ValueTuple<int, int> direction = currInstruction switch
            {
                '^' => (-1, 0),
                'v' => (1, 0),
                '>' => (0, 1),
                '<' => (0, -1),
                _ => throw new InputInvalidException($"Invalid instruction: {currInstruction}")
            };
            if (!TryMove(map, robot, direction, out HashSet<ValueTuple<int, int, char>> movedBoxes, stage))
            {
                continue;
            }
            foreach ((int, int, char) box in movedBoxes)
            {
                map[box.Item1][box.Item2] = '.';
            }
            foreach ((int, int, char) box in movedBoxes)
            {
                map[box.Item1+direction.Item1][box.Item2+direction.Item2] = box.Item3;
            }
            robot = (robot.Item1 + direction.Item1, robot.Item2 + direction.Item2);
        }
        return GetCoordinates(map, stage == 1 ? 'O' : '[');
    }
    
    private static bool TryMove(
        List<char[]> map, 
        ValueTuple<int, int> start, 
        ValueTuple<int, int> direction, 
        out HashSet<ValueTuple<int, int, char>> movedBoxes,
        int stage
    )
    {
        Queue<ValueTuple<int, int>> queue = new();
        movedBoxes = [];
        queue.Enqueue((start.Item1+direction.Item1, start.Item2+direction.Item2));
        while (queue.TryDequeue(out ValueTuple<int, int> box))
        {
            switch (map[box.Item1][box.Item2])
            {
                case '#':
                    movedBoxes.Clear();
                    return false;
                case '.':
                    continue;
            }
            if(!movedBoxes.Add((box.Item1, box.Item2, map[box.Item1][box.Item2])))
            { 
                continue;
            }
            if (stage == 2)
            {
                queue.Enqueue((box.Item1, box.Item2 + (map[box.Item1][box.Item2] == ']' ? -1 : 1)));   
            }
            queue.Enqueue((box.Item1+direction.Item1, box.Item2+direction.Item2));
        }
        return true;
    }
    
    public override object PartOne()
    {
        string[] input = ReadInput();
        ParseInput(input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, 1);
        return PerformInstructions(map, instructions, robot, 1);
    }

    public override object PartTwo()
    {
        string[] input = ReadInput();
        ParseInput(input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, 2);
        return PerformInstructions(map, instructions, robot, 2);
    }


    public override void Reset()
    {

    }
}

