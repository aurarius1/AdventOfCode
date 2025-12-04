
using System.Diagnostics;
using System;



namespace _2025._02;



public sealed class Day02 : Base
{
    public Day02(bool example) : base(example)
    {
        Day = "02";
    }

    private IEnumerable<(ulong, ulong)> Parse(string[] input)
    {
        return input.Select(x =>
        {
            var split = x.Split("-");
            return (ulong.Parse(split[0]), ulong.Parse(split[1]));
        });
    }
    
    public override object PartOne()
    {
        string[] input = ReadInput()[0].Split(",");
        ulong invalidIds = 0;
        foreach((ulong lower, ulong upper) in Parse(input))
        {
            for(ulong it = lower; it <= upper; it++)
            {
                string id = it.ToString();
                if (id.Length % 2 != 0)
                {
                    continue;
                }

                if (id[0..(id.Length/2)] == id[(id.Length/2)..])
                {
                    invalidIds += it;
                }
            }
        }
        return invalidIds.ToString();
    }

    public override object PartTwo()
    {
        string[] input = ReadInput()[0].Split(",");
        ulong invalidIds = 0;
        foreach((ulong lower, ulong upper) in Parse(input))
        {
            for(ulong it = lower; it <= upper; it++)
            {
                string id = it.ToString();
                for(int idx = 1; idx <= id.Length/2; idx++)
                {
                    string first = id[0..idx];
                    bool repeats = true;

                    if(id.Length % idx != 0) 
                    {
                        continue;
                    }

                    for(int check = idx; check < id.Length; check += idx)
                    {
                        if(id[check..(check+idx)] != first)
                        {
                            repeats = false;
                            break;
                        }
                    }

                    if (repeats)
                    {
                        invalidIds += it;
                        break;
                    }
                    
                }
            }
        }
        return invalidIds.ToString();
    }
}