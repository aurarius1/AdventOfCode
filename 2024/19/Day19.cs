
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._19;



public class Day19 : Base
{
    private Dictionary<string, long> Cache { get; } = new();
    private List<string> AvailableTowels { get; set; } = [];
    private List<string> NeededPatterns { get; set; } = [];
    
    public Day19(bool example) : base(example)
    {
        Day = "19";
        ParseInput();
    }

    private void ParseInput()
    {
        string[] input = ReadInput();
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
                    NeededPatterns.Add(line);
                    break;
            }
        }
    }
    
    private long IsPatternPossible(string pattern)
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

        Cache[pattern] = 0;
        // before I had pattern.StartsWith(availableTowel), this is roughly 2000ms slower than pattern[..availableTowel.Length] == availableTowel
        foreach (string availableTowel in AvailableTowels.Where(availableTowel => availableTowel.Length <= pattern.Length && pattern[..availableTowel.Length] == availableTowel))
        {
            string subPattern = pattern[availableTowel.Length..];
            Cache[pattern] += IsPatternPossible(subPattern);
        }
        // loop above could be replaced with this linq expression: 
        /*
        Cache[pattern] = AvailableTowels
            .Where(availableTowel =>
                availableTowel.Length <= pattern.Length && pattern[..availableTowel.Length] == availableTowel)
            .Select(availableTowel => IsPatternPossible(pattern[availableTowel.Length..])).Sum();
        */
        return Cache[pattern];
    }
    
    public override object PartOne()
    {
        return NeededPatterns.Count(neededPattern => IsPatternPossible(neededPattern) > 0);
    }
    
    public override object PartTwo()
    {
        return NeededPatterns.Sum(IsPatternPossible);
    }

    public override void Reset()
    {
        // removing this line makes part 2 use the cache build from part 1
        // effectively making it run instantly 
        Cache.Clear();
    }
}

