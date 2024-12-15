
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
    public Day15()
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
            bool movePossible;
            HashSet<ValueTuple<int, int, char>> movedBoxes;
            ValueTuple<int, int> direction;
            switch (currInstruction)
            {
                case '^':
                    direction = (-1, 0);
                    movePossible = TryMoveVertical(map, robot, direction, out movedBoxes, stage);
                    break;
                case 'v':
                    direction = (1, 0);
                    movePossible = TryMoveVertical(map, robot, direction, out movedBoxes, stage);
                    break;
                case '>':
                    direction = (0, 1);
                    movePossible = TryMoveHorizontal(map, robot, direction, out movedBoxes);
                    break;
                case '<':
                    direction = (0, -1);
                    movePossible = TryMoveHorizontal(map, robot, direction, out movedBoxes);
                    break;
                default:
                    throw new InputInvalidException($"Invalid instruction: {currInstruction}");
                    
            }
            
            if (!movePossible)
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
    
    private static bool TryMoveVertical(
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

    // vertical movement can always be handled the same for both parts 
    // because the original character is stored, and we just move everything one to the left, we just need to check 
    // if there is a space at the end, the exact arrangement of boxes doesn't matter
    // the additional stage parameter is just to be able to reuse the same delegate method template
    private static bool TryMoveHorizontal(
        List<char[]> map,
        ValueTuple<int, int> start,
        ValueTuple<int, int> direction,
        out HashSet<ValueTuple<int, int, char>> movedBoxes
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
            ValueTuple<int, int> next = (box.Item1+direction.Item1, box.Item2+direction.Item2);
            queue.Enqueue(next);
        }
        return true;
    }
    
    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        ParseInput(input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, 1);
        return PerformInstructions(map, instructions, robot, 1);
    }

    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        ParseInput(input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, 2);
        return PerformInstructions(map, instructions, robot, 2);
    }


    public override void Reset()
    {

    }
}

