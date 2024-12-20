using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._07;



public class Day07 : Base
{
    public Day07(bool example) : base(example) 
    {
        Day = "7";
    }

    private int ChooseOperator(ulong result, List<ulong> numbers)
    {
        int placements = 0;

        if (numbers.Count <= 1)
        {
            return 0;
        }
        ulong test1 = numbers[0] + numbers[1];
        ulong test2 = numbers[0] * numbers[1];

        placements += (test1 == result) ? 1 : 0;
        placements += (test2 == result) ? 1 : 0;
        
        
        List<ulong> tmp = new (numbers[1..])
        {
            [0] = test1
        };
        placements += ChooseOperator(result, tmp);

        tmp[0] = test2;
        placements += ChooseOperator(result, tmp);
        
        return placements;
    }


    private static ulong PerformOperation(ulong n1, ulong n2, int operation)
    {
        return operation switch
        {
            0 => n1 + n2,
            1 => n1 * n2,
            2 => ulong.Parse($"{n1}{n2}"),
            _ => throw new InvalidOperationException()
        };
    }
    
    
    private bool IsResultPossible(ulong result, List<ulong> numbers, int stage = 1)
    {
        int[] operators = new int[numbers.Count-1];
        for (int count = 0; count < Math.Pow(stage+1, numbers.Count - 1); count++)
        {
            ulong curr = PerformOperation(numbers[0], numbers[1], operators[0]);
            for(int i = 2; i < numbers.Count; i++)
            {
                curr = PerformOperation(curr, numbers[i], operators[i-1]);
            }

            if (curr == result)
            {
                // we are only interested in if any operator combination can solve the puzzle 
                // if we found such a combination, end search
                return true;
            }

            int overflow = 1;
            for (int i = 0; i < operators.Length; i++)
            {
                operators[i] += overflow;
                overflow = 0;
                if (operators[i] != stage+1)
                {
                    continue;
                }
                overflow = 1;
                operators[i] = 0;
            }
        }
        return false;
    }
    
    public override object PartOne()
    {
        string[] input = ReadInput();
        ulong totalCalibrationResult = 0;
        foreach (string line in input)
        {
            string[] split = line.Split(": ");
            ulong testValue = ulong.Parse(split[0]);
            List<ulong> numbers = split[1].Split(" ").Select(ulong.Parse).ToList();
            if (IsResultPossible(testValue, numbers))
            {
                totalCalibrationResult += testValue;
            }
        }
        return totalCalibrationResult;
    }
    
    public override object PartTwo()
    {
        string[] input = ReadInput();
        ulong totalCalibrationResult = 0;
        foreach (string line in input)
        {
            string[] split = line.Split(": ");
            ulong testValue = ulong.Parse(split[0]);
            List<ulong> numbers = split[1].Split(" ").Select(ulong.Parse).ToList();
            if (IsResultPossible(testValue, numbers, 2))
            {
                totalCalibrationResult += testValue;
            }
        }
        return totalCalibrationResult;
    }
}