
using System.Diagnostics;
using System;
using _2025.Utils;

namespace _2025._01;

public enum Rotation
{
    LEFT,
    RIGHT
}

public sealed class Day01 : Base
{
    public Day01(bool example) : base(example)
    {
        Day = "01";
    }

    public IEnumerable<(Rotation, int)> Parse(string[] input)
    {
        return input.Select(x => (x[0] == 'L' ? Rotation.LEFT : Rotation.RIGHT, int.Parse(x[1..])));
    }
    
    public override object PartOne()
    {
        string[] input = ReadInput();
        int dialPosition = 50;
        int password = 0;
        foreach((Rotation rotation, int steps) in Parse(input))
        {
            int dir = rotation == Rotation.LEFT ? -steps : steps;
            dialPosition = MathExtensions.Modulo(dialPosition+dir, 100); 
            if(dialPosition == 0)
            {
                password++;
            }
        }
        return password.ToString(); 
    }

    public override object PartTwo()
    {
        string[] input = ReadInput();
        int dialPosition = 50;
        int password = 0;
        foreach((Rotation rotation, int steps) in Parse(input))
        {
            if(rotation == Rotation.LEFT)
            {
                // wrap around first to prevent counting starting from zero as hitting a zero once
                if(dialPosition == 0)
                {
                    dialPosition = 100;
                }
                dialPosition -= steps;
                password += (int)Math.Ceiling((dialPosition*-1) / 100.0f);
                dialPosition = MathExtensions.Modulo(dialPosition+steps, 100);
                
            } else
            {
                password += (int)Math.Floor((dialPosition + steps) / 100.0f);
                dialPosition = MathExtensions.Modulo(dialPosition+steps, 100);
            }
        }
        return password.ToString(); 
    }
}