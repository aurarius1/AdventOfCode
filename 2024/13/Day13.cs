
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._13;

public class Day13 : Base
{
    public Day13(bool example) : base(example) 
    {
        Day = "13";
    }

    private static (long, long) GetXAndY(string input)
    {
        long[] parsed = Regex.Replace(input, @"(Prize: |Button A: |Button B: |X=|Y=|X\+|Y\+)", "")
            .Split(", ")
            .Select(long.Parse)
            .ToArray();
        return (parsed[0], parsed[1]);
    }

    private static long SolvePuzzle(string[] input, int stage)
    {
        long offset = stage == 1 ? 0 : 10000000000000;
        const int tokensA = 3, tokensB = 1;

        long cost = 0;
        for (int machine = 0; machine < input.Length; machine += 4)
        {
            // https://math.stackexchange.com/questions/21533/shortcut-for-finding-a-inverse-of-matrix
            (long a, long c) = GetXAndY(input[machine]);
            (long b, long d) = GetXAndY(input[machine+1]);
            (long, long) prize = GetXAndY(input[machine+2]);
            long det = a*d - c*b;
            if (det == 0)
            {
                continue;
            }
            prize = (prize.Item1 + offset, prize.Item2 + offset);
            long solA = (long)Math.Round(((double)d * prize.Item1 + (double)-b * prize.Item2)/det);
            long solB = (long)Math.Round(((double)-c * prize.Item1 + (double)a * prize.Item2)/det);
            
            if ((solA * a + solB * b) != prize.Item1 || (solA * c + solB * d) != prize.Item2)
            {
                continue;
            }

            if (stage == 1 && (solA > 100 || solB > 100))
            {
                continue;
            }
            
            cost += (solA*tokensA) + (solB*tokensB); 
        }

        return cost;
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


    public override void Reset()
    {

    }
}

