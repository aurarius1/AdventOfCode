
using _2024.Utils;

namespace _2024._17;


public class Day17 : Base
{
    public Day17(bool example) : base(example) 
    {
        Day = "17";
    }

    private static long GetComboOperand(int operand, List<long> registers)
    {
        return operand switch
        {
            >= 0 and <= 3 => operand,
            4 => registers[0],
            5 => registers[1],
            6 => registers[2],
            _ => throw new InputInvalidException("Operand invalid")
        };
    }

    private string RunProgram(List<int> instructions, List<long> registers, int stage)
    {
        List<long> output = [];
        for (int i = 0; i < instructions.Count;)
        {
            switch (instructions[i])
            {
                case 0:
                    registers[0] = (long)(registers[0] / Math.Pow(2, GetComboOperand(instructions[i + 1], registers)));
                    i += 2;
                    break;
                case 1:
                    registers[1] ^= instructions[i + 1];
                    i += 2;
                    break;
                case 2:
                    registers[1] = MathExtensions.Modulo(GetComboOperand(instructions[i + 1], registers), 8);
                    i += 2;
                    break;
                case 3:
                    if (registers[0] == 0)
                    {
                        i += 2;
                        continue;
                    }
                    i = instructions[i + 1];
                    break;
                case 4:
                    registers[1] ^= registers[2];
                    i += 2;
                    break;
                case 5:
                    output.Add(GetComboOperand(instructions[i + 1], registers) % 8);
                    if (stage == 2)
                    {
                        return output[^1].ToString();
                    }
                    i += 2;
                    break;
                case 6:
                    registers[1] = (long)(registers[0] / Math.Pow(2, GetComboOperand(instructions[i + 1], registers)));
                    i += 2;
                    break;
                case 7:
                    registers[2] = (long)(registers[0] / Math.Pow(2, GetComboOperand(instructions[i + 1], registers)));
                    i += 2;
                    break;
                default:
                    throw new InputInvalidException($"Opcode: {instructions[i]} is not valid");
            }
        }
        return string.Join(",", output);
    }

    public override object PartOne()
    {
        string[] input = ReadInput();

        // three registers
        List<long> registers = new List<long>(3);
        List<int> instructions = [];

        foreach(string line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            string[] split = line.Split(": ");
            if (line.StartsWith("Register"))
            {   
                registers.Add(long.Parse(split[1]));
            }
            if (line.StartsWith("Program"))
            {
                instructions = split[1].Split(",").Select(int.Parse).ToList();
            }
        }

        return RunProgram(instructions, registers, 1);
    }

    public override object PartTwo()
    {
        string[] input = ReadInput();
        // three registers
        List<long> registers = new(3);
        List<int> instructions = [];

        foreach (string line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            string[] split = line.Split(": ");
            if (line.StartsWith("Register"))
            {
                registers.Add(long.Parse(split[1]));
            }
            if (line.StartsWith("Program"))
            {
                instructions = split[1].Split(",").Select(int.Parse).ToList();
            }
        }

        int currInstruction = instructions.Count - 1;
        List<long> candidates = [0];
        while (currInstruction >= 0)
        {
            List<long> newCandidates = new(8*candidates.Count);
            foreach (long candidate in candidates)
            {
                for (long i = 0; i < 8; i++)
                {
                    registers[0] = (candidate * 8) + i;
                    registers[1] = 0;
                    registers[2] = 0;
                    string output = RunProgram(instructions, registers, 2);
                    if (output == instructions[currInstruction].ToString())
                    {
                        newCandidates.Add((candidate * 8) + i);
                    }
                }
            }
            candidates = [..newCandidates];
            currInstruction--;
        }
        return candidates.Min();
    }

    public override void Reset()
    {

    }
}

