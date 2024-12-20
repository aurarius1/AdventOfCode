using System.Text.RegularExpressions;
using _2024.Utils;

namespace _2024._03;

public class Day03 : Base
{
    public Day03(bool example) : base(example)
    {
        Day = "3";
    }

    public override object PartOne()
    {
        string[] input = ReadInput();
        int solution = 0;
        Regex pattern = new Regex("mul\\([0-9]{1,3},[0-9]{1,3}\\)");
        MatchCollection matches = pattern.Matches(String.Join("", input));
        foreach (Match match in matches)
        {
            string[] numbers = match.Value.Replace("mul(", "").Replace(")", "").Split(",");
            int a = int.Parse(numbers[0]);
            int b = int.Parse(numbers[1]);
            solution += (a * b);
        }
        return solution;
    }

    public override object PartTwo()
    {
        string[] input = ReadInput();
        int solution = 0;
        bool mulEnabled = true;
        Regex pattern = new Regex("mul\\([0-9]{1,3},[0-9]{1,3}\\)|don't\\(\\)|do\\(\\)");
        MatchCollection matches = pattern.Matches(String.Join("", input));
        foreach (Match match in matches)
        {
           
            switch (match.Value)
            {
                case "do()":
                    mulEnabled = true;
                    break;
                case "don't()":
                    mulEnabled = false;
                    break;
                default:
                    string[] numbers = match.Value.Replace("mul(", "").Replace(")", "").Split(",");
                    int a = int.Parse(numbers[0]);
                    int b = int.Parse(numbers[1]);
                    solution += mulEnabled ? (a * b) : 0;
                    break;
            }
        }

        return solution;
    }
}