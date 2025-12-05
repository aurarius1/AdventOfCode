using System.Diagnostics;
using System;


namespace _2025._04
{
    public sealed class Day04 : Base
    {
        private string[] _input = [];
        public Day04(bool example) : base(example)
        {
            Day = "04";
        }

        private bool PaperRollAccessible(int row, int col)
        {
            if (_input[row][col] != '@')
            {
                return false;
            }
            var steps = new []{(-1, -1), (-1, 0), (0, -1), (1, 0), (0, 1), (1, 1), (-1, 1), (1, -1)};
            return steps.Select(x => 
                (_input.TryGetValue((row + x.Item1, col + x.Item2), out var value) && value == '@') ? 1 : 0
            ).Sum() < 4;
        }

        public override object PartOne()
        {
            _input = ReadInput();
            int accessibleRolls = 0;
            for(int row = 0; row < _input.Length; row++)
            {
                for(int col = 0; col < _input[row].Length; col++)
                {
                    if (PaperRollAccessible(row, col))
                    {
                        accessibleRolls++;
                    } 
                }
            }
            return accessibleRolls;
        }

        public override object PartTwo()
        {
            _input = ReadInput();
            int accessibleRolls = 0, currAccessible;
            do
            {
                currAccessible = 0;
                for (int row = 0; row < _input.Length; row++)
                {
                    for (int col = 0; col < _input[row].Length; col++)
                    {
                        if (!PaperRollAccessible(row, col))
                        {
                            continue;
                        }
                        currAccessible++;
                        _input[row] = _input[row][..col] + "." + _input[row][(col + 1)..];
                    }
                }
                accessibleRolls += currAccessible;
            } while (currAccessible > 0);
            return accessibleRolls;
        }
    }
}
