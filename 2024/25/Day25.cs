
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._25;



public class Day25 : Base
{
    private readonly List<List<int>> _locks = [];
    private readonly List<List<int>> _keys = [];
    
    const int rows = 7;

    public Day25(bool example) : base(example)
    {
        Day = "25";

        string[] input = ReadInput();
        
        for (int schematic = 0; schematic < input.Length; schematic += rows+1)
        {
            // 
            List<int> currSchematic = 
            [
                0, 0, 0, 0, 0,
            ]; 
            for (int row = 0; row < rows; row++)
            {
                foreach((char c, int idx) in input[schematic + row].Enumerate())
                {
                    if (c == '#')
                    {
                        currSchematic[idx]++;
                    }
                }
            }

            // key
            if (input[schematic].Contains('.'))
            {
                _keys.Add(currSchematic);
            }
            // lock
            else
            {
                _locks.Add(currSchematic);
            }
        }
    }

    private static bool Overlap(int lockPin, int keyHeight) => lockPin - 1 > (rows - keyHeight - 1);
    
    public override object PartOne()
    {
        return _locks.Sum(_lock => _keys.Count(key => !_lock.Zip(key, Overlap).Any(o => o)));
    }

    public override object PartTwo()
    {
        return "You did it.";
    }
       

    public override void Reset()
    {
    }
}

