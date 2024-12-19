
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._19;



public class Day19 : Base
{
    public Day19()
    {
        Day = "19";
    }

    private Dictionary<string, long> Cache { get; set; }= new();
    private List<string> AvailableTowels { get; set; } = [];

    private long IsPatternPossible(string pattern, int stage)
    {
        if (Cache.TryGetValue(pattern, out long possible))
        {
            return possible;
        }
        
        if (pattern.Length == 0)
        {
            Cache[pattern] = 1;
            return 1;
        }

        long combinations = 0;
        foreach (string availableTowel in AvailableTowels.Where(availableTowel => availableTowel.Length <= pattern.Length && pattern.StartsWith(availableTowel)))
        {
            string subPattern = pattern[availableTowel.Length..];
            combinations += IsPatternPossible(subPattern, stage);

        }

        Cache[pattern] = combinations;
        return combinations;
    }


    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        
        List<string> neededPatterns = [];

        foreach ((string line, int idx) in input.Enumerate())
        {
            switch (idx)
            {
                case 0:
                    AvailableTowels = [..line.Split(", ")];
                    continue;
                case 1:
                    continue;
                default:
                    neededPatterns.Add(line);
                    break;
            }
        }

        return neededPatterns.Count(neededPattern => IsPatternPossible(neededPattern, 1) != 0);
    }
    
    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        Cache.Clear();
        List<string> neededPatterns = [];

        foreach ((string line, int idx) in input.Enumerate())
        {
            switch (idx)
            {
                case 0:
                    AvailableTowels = [..line.Split(", ")];
                    continue;
                case 1:
                    continue;
                default:
                    neededPatterns.Add(line);
                    break;
            }
        }

        return  neededPatterns.Sum(neededPattern => IsPatternPossible(neededPattern, 2));
    }

    public override void Reset()
    {
        AvailableTowels.Clear();
    }
}

