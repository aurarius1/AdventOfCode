using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._11;

public class Day11 : Base
{
    public Day11(bool example) : base(example) 
    {
        Day = "11";
    }

    private static ValueTuple<ulong, ulong> SplitStone(ulong stone)
    {
        ulong digits = (ulong)Math.Log10(stone) + 1;
        ulong divisor = (ulong)Math.Pow(10, (digits / 2)); 
        ulong firstStone = stone / divisor;
        ulong secondStone = stone % divisor;         
        return (firstStone, secondStone);
    }
    
    private static void SafeAdd(ulong stone, Dictionary<ulong, ulong> stones, ulong toAdd)
    {
        stones.TryAdd(stone, 0);
        stones[stone] += toAdd;
    }

    private static ulong SolvePuzzle(string input, int stage)
    {
        Dictionary<ulong, ulong> stones = input.Split(' ').Select(ulong.Parse)
            .GroupBy(num => num) 
            .ToDictionary(g => g.Key, g => (ulong)g.Count());
        
        for (int i = 0; i < ((stage == 1) ? (25) : (75)); i++)
        {
            Dictionary<ulong, ulong> stonesAfterBlink = new(stones);
            foreach (ulong stone in stones.Keys.Where(stone => stones[stone] != 0))
            {
                stonesAfterBlink[stone] -= stones[stone];
                
                if (stone == 0)
                {
                    SafeAdd(stone+1, stonesAfterBlink, stones[stone]);
                    continue;
                }
                
                // if num digits is even, split
                if ((((ulong)Math.Log10(stone) + 1) % 2) == 0)
                {
                    (ulong first, ulong second) = SplitStone(stone);
                    SafeAdd(first, stonesAfterBlink, stones[stone]);
                    SafeAdd(second, stonesAfterBlink, stones[stone]);
                    continue;
                }

                SafeAdd(stone * 2024, stonesAfterBlink, stones[stone]);
            }
            
            stones = stonesAfterBlink;
        }

        return stones.Keys.Aggregate<ulong, ulong>(0, (current, stone) => (current + stones[stone]));

    }
    
    
    public override object PartOne()
    {
        string[] input = ReadInput();
        return SolvePuzzle(input[0], 1);
    }

    public override object PartTwo()
    {
        string[] input = ReadInput();
        return SolvePuzzle(input[0], 2);
    }
}