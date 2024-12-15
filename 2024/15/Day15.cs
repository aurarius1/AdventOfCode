
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
                map.Add(line.ToCharArray());
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


    private static bool TryMove(
        List<char[]> map, 
        ValueTuple<int, int> robot, 
        ValueTuple<int, int> direction, 
        int stage,
        out int steps
        )
    {
        steps = 1;

        char[] boxCharacters = stage == 1 ? ['O'] : [']', '['];
        
        while (boxCharacters.Contains(map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps]))
        {
            steps++;
        }
            
        // robot cant move, obstacles in this direction
        if (map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] != '#')
        {
            return true;
        };
        steps = -1;
        return false;
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
    
    private static int PerformInstructions(List<char[]> map, List<char> instructions, ValueTuple<int, int> robot)
    {
        foreach(char instruction in instructions)
        {
            ValueTuple<int, int> direction = instruction switch
            {
                '<' => (0, -1),
                '>' => (0, 1),
                'v' => (1, 0),
                '^' => (-1, 0),
                _ => (0, 0)
            };

            switch (map[robot.Item1 + direction.Item1][robot.Item2 + direction.Item2])
            {
                case '#':
                    continue;
                case '.':
                    robot = (robot.Item1 + direction.Item1, robot.Item2 + direction.Item2);
                    continue;
            }

            if (!TryMove(map, robot, direction, 1, out int steps))
            {
                continue;
            }
            while (steps > 0)
            {
                map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] = 'O';
                steps--;
            }
            robot = (robot.Item1 + direction.Item1, robot.Item2 + direction.Item2);
            // this is the field the robot would be at
            map[robot.Item1][robot.Item2] = '.';
        }
        return GetCoordinates(map, 'O');
    }
    
    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        ParseInput(input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, 1);
        return PerformInstructions(map, instructions, robot);
    }


    private static int TestMoveHorizontal(
        List<char[]> map, 
        ValueTuple<int, int> start, 
        ValueTuple<int, int> direction, 
        Dictionary<int, List<ValueTuple<int, int, char>>> movedBoxes
    )
    {
        ValueTuple<int, int> box1 = (start.Item1+direction.Item1, start.Item2+direction.Item2);
        switch (map[box1.Item1][box1.Item2])
        {
            case '#':
                return -1;
            case '.':
                return 1;
        }
        ValueTuple<int, int> box2 = (box1.Item1, box1.Item2 + (map[box1.Item1][box1.Item2] == ']' ? -1 : 1));
        int left = TestMoveHorizontal(map, box1, direction, movedBoxes);
        int right = TestMoveHorizontal(map, box2, direction, movedBoxes);

        if (left == -1 || right == -1)
        {
            return -1;
        };
        movedBoxes.TryAdd(box1.Item1, []);
        movedBoxes[box1.Item1].Add((box1.Item1, box1.Item2, map[box1.Item1][box1.Item2]));
        movedBoxes[box2.Item1].Add((box2.Item1, box2.Item2, map[box2.Item1][box2.Item2]));
        return 1+left;

    }

    
    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        ParseInput(input, out List<char[]> map, out List<char> instructions, out ValueTuple<int, int> robot, 2);

        foreach((char instruction, int cnt) in instructions.Enumerate())
        {
            ValueTuple<int, int> direction = instruction switch
            {
                '<' => (0, -1),
                '>' => (0, 1),
                'v' => (1, 0),
                '^' => (-1, 0),
                _ => (0, 0)
            };

            switch (map[robot.Item1 + direction.Item1][robot.Item2 + direction.Item2])
            {
                case '#':
                    continue;
                case '.':
                    robot = (robot.Item1 + direction.Item1, robot.Item2 + direction.Item2);
                    continue;
            }

            int steps = 0;
            if (direction == (-1, 0) || direction == (1, 0))
            {
                Dictionary<int, List<ValueTuple<int, int, char>>> movedBoxes = new();
                steps = TestMoveHorizontal(map, robot, direction, movedBoxes);
                if (steps == -1)
                {
                    continue;
                }
                foreach ((int, int, char) box in movedBoxes.Values.SelectMany(boxes => boxes))
                {
                    map[box.Item1][box.Item2] = '.';
                }
                foreach ((int, int, char) box in movedBoxes.Values.SelectMany(boxes => boxes))
                {
                    map[box.Item1+direction.Item1][box.Item2] = box.Item3;
                }
                robot = (robot.Item1 + direction.Item1, robot.Item2 + direction.Item2);
                continue;
            }
            
            
            
            steps = 1;
            while (map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] == ']' || 
                   map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] == '[')
            {
                steps += 2;
            }
            //robot cant move obstacles in this direction
            if (map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] == '#')
            {
                continue;
            }
           
            while (steps > 1)
            {
                map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] = map[robot.Item1 + direction.Item1 *
                    (steps-1)][robot.Item2 + direction.Item2 * (steps-1)];
                steps--;
                map[robot.Item1 + direction.Item1 * steps][robot.Item2 + direction.Item2 * steps] = map[robot.Item1 + direction.Item1 *
                    (steps-1)][robot.Item2 + direction.Item2 * (steps-1)];
                steps--;
            }
            robot = (robot.Item1 + direction.Item1, robot.Item2 + direction.Item2);
            map[robot.Item1][robot.Item2] = '.';
        }
        return GetCoordinates(map, '[');
    }


    public override void Reset()
    {

    }
}

