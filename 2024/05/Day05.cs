using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;

namespace _2024._05;

public class Day05 : Base
{
    public Day05()
    {
        Day = "5";
    }

    private int ReadRules(string[] input, out List<ValueTuple<int, int>> rules)
    {
        rules = [];
        foreach (var (line, idx) in input.Enumerate())
        {
            if (string.IsNullOrEmpty(line))
            {
                return idx+1;
            }
            string[] numbers = line.Split("|");
            rules.Add((int.Parse(numbers[0]), int.Parse(numbers[1])));
        }

        return -1;
    }
    
    
    private bool IsUpdateCorrect(string[] update, List<ValueTuple<int, int>> rules)
    {
        HashSet<int> seenNumbers = [];
        int[] updateNumbers = new int[update.Length];
        for (int i = 0; i < update.Length; i++)
        {
            updateNumbers[i] = int.Parse(update[i]);
        }
        
        foreach (var (number, idx) in updateNumbers.Enumerate())
        {
            seenNumbers.Add(number);
            var numberSecondRules = rules.Where(x => x.Item2 == number);

            foreach (var (first, second) in numberSecondRules)
            {
                if (updateNumbers.Any(x => x == first) && !seenNumbers.Contains(first))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static int SortByRules(int x, int y, List<ValueTuple<int, int>> rules)
    {
        var necessaryRules = rules.FirstOrDefault(rule => rule.Item1 == y && rule.Item2 == x);
        if (necessaryRules == default)
        {
            return -1;
        }
        return 1;
    }
    
    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        int middleNumbersSum = 0;
        int updatesStart = ReadRules(input, out List<(int, int)> orderRules);
        for (int i = updatesStart; i < input.Length; i++)
        {
            string[] line = input[i].Split(",");
            if (IsUpdateCorrect(line, orderRules))
            {
                middleNumbersSum += int.Parse(line[line.Length / 2]);
            }
        }
        return middleNumbersSum;
    }

    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        int middleNumbersSum = 0;
        int updatesStart = ReadRules(input, out List<(int, int)> orderRules);
        for (int i = updatesStart; i < input.Length; i++)
        {
            string[] line = input[i].Split(",");
            if (IsUpdateCorrect(line, orderRules))
            {
                continue;
            }
            
            List<int> numbers = line
                .Select(int.Parse)
                .OrderBy(x => x, Comparer<int>.Create((x, y) => SortByRules(x, y, orderRules)))
                .ToList();
            middleNumbersSum += numbers[numbers.Count / 2];
        }
        
        return middleNumbersSum;
    }
}