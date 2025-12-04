using System.Diagnostics;
using System;


namespace _2025._04
{
    public sealed class Day04 : Base
    {
        public Day04(bool example) : base(example)
        {
            Day = "04";
        }

        private string[] _input;
        

        private bool RollAccessible(int row, int col, int rows, int cols)
        {
            int adjacentRolls = 0;
            var steps = new (int, int)[]{(-1, -1), (-1, 0), (0, -1), (1, 0), (0, 1), (1, 1), (-1, 1), (1, -1)};
            foreach((int stepRow, int stepCol) in steps)
            {
                int newRow = row + stepRow;
                int newCol = col + stepCol; 
                if(_input.TryGetValue((row+stepRow, col+stepCol), out char? value) && value == '@')
                {
                    adjacentRolls++;
                }
            }
            return adjacentRolls < 4;
        }

        public override object PartOne()
        {
            _input = ReadInput();
            int accessibleRolls = 0;
            for(int row = 0; row < _input.Length; row++)
            {
                for(int col = 0; col < _input[row].Length; col++)
                {
                    if (_input[row][col] == '@' && RollAccessible(row, col, _input.Length, _input[row].Length))
                    {
                        accessibleRolls++;
                    } 
                }
            }
            return accessibleRolls.ToString();
        }

        public override object PartTwo()
        {
            _input = ReadInput();
            int accessibleRolls = 0;
            while (true)
            {   
                int currAccessible = 0;
                for(int row = 0; row < _input.Length; row++)
                {
                    for(int col = 0; col < _input[row].Length; col++)
                    {
                        if (_input[row][col] == '@' && RollAccessible(row, col, _input.Length, _input[row].Length))
                        {
                            currAccessible++;
                            _input[row] = _input[row].Substring(0, col) + "." + _input[row].Substring(col + 1);

                        } 
                    }
                }
                if(currAccessible == 0)
                {
                    break;
                }
                accessibleRolls += currAccessible;
            }
            return accessibleRolls.ToString();
        }
    }
}
