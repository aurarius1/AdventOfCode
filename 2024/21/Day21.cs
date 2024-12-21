
using System.Collections.Immutable;
using System.Data;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._21;



public class Day21 : Base
{
    private static readonly List<char[]> NumericPad =
        [
            ['7', '8', '9'],
            ['4', '5', '6'],
            ['1', '2', '3'],
            [' ', '0', 'A']
        ];
    private static readonly List<char[]> DirectionalPad =
        [
            [' ', '^', 'A'],
            ['<', 'v', '>']
        ];
    private static readonly ValueTuple<ValueTuple<int, int>, char>[] Directions = 
        [
            ((0, 1), '>'), 
            ((0, -1), '<'), 
            ((1, 0), 'v'), 
            ((-1, 0), '^')
        ];
    private static ValueTuple<int, int> NumericPadStart => (NumericPad.Count - 1, NumericPad[^1].Length - 1);
    private static ValueTuple<int, int> DirectionalPadStart => (0, DirectionalPad[0].Length - 1);
    
    private readonly string[] _codes;
    private readonly Dictionary<ValueTuple<string, int>, long> _cache = new();
    private int _numRobots = 2;

    public Day21(bool example) : base(example)
    {
        Day = "21";
        _codes = ReadInput();
    }
    
    
    private static HashSet<string> FindShortestSequences(bool isDirectional, string sequence, ValueTuple<int, int> start)
    {
        PriorityQueue<ValueTuple<ValueTuple<int, int>, int, string>, int> queue = new();
        Dictionary<ValueTuple<ValueTuple<int, int>, int>, int> visited = new();
        queue.Enqueue((start, 0, ""), 0);
        List<char[]> pad = isDirectional ? DirectionalPad : NumericPad;
        HashSet<string> bestSequences = [];
        
        while (queue.TryDequeue(out ((int, int), int, string) curr, out int moves))
        {
            (ValueTuple<int, int> pos, int sequencePos, string buildSequence) = curr;
            if (sequencePos == sequence.Length)
            {
                bestSequences.Add(buildSequence);
                continue;
            }

            if (visited.TryGetValue((pos, sequencePos), out int stepsTaken) && stepsTaken < moves)
            {
                continue;
            }

            visited[(pos, sequencePos)] = moves;
            
            if (pad.TryGetValue(pos, out char value) && value == sequence[sequencePos])
            {
                queue.Enqueue((pos, sequencePos + 1, buildSequence+'A'), moves + 1);
                continue;
            }

            foreach ((ValueTuple<int, int> direction, char symbol) in Directions)
            {
                ValueTuple<int, int> newPos = (pos.Item1 + direction.Item1, pos.Item2 + direction.Item2);
                if (!pad.TryGetValue(newPos, out value) || value == ' ')
                {
                    continue;
                }
                queue.Enqueue((newPos, sequencePos, buildSequence+symbol), moves + 1);
            }
        }
        return bestSequences;
    }
    
    private static ValueTuple<int, int> GetEnd(bool isDirectional, char code)
    {
        List<char[]> pad = (isDirectional ? DirectionalPad : NumericPad);
        int row = pad.FindIndex(x => x.Contains(code));
        return (row, Array.IndexOf(pad[row], code));
    }
    
    private long GetShortestSequence(string code, int position = 0)
    {
        if (_cache.TryGetValue((code, position), out long result))
        {
            return result;
        }
        if (position > _numRobots)
        {
            return code.Length;
        }

        bool isDirectional = position > 0;
        
        
        ValueTuple<int, int> start = isDirectional ? DirectionalPadStart : NumericPadStart;
        long pathLength = 0;
        foreach (char t in code)
        {
            HashSet<string> sequences = FindShortestSequences(isDirectional, t.ToString(), start);
            start = GetEnd(isDirectional, t);
            pathLength += sequences.Select(sequence => GetShortestSequence(sequence, position + 1)).Min();
        }
        _cache.Add((code, position), pathLength);
        return pathLength;
    }
    
    public override object PartOne()
    {
        _numRobots = 2;
        return _codes.Select(code => GetShortestSequence(code) * long.Parse(code[..^1])).Sum();
    }
    
    public override object PartTwo()
    {
        _numRobots = 25;
        return _codes.Select(code => GetShortestSequence(code) * long.Parse(code[..^1])).Sum();
    }

    public override void Reset()
    {
        _cache.Clear();
    }
}

