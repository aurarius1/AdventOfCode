using _2024.Utils;

namespace _2024._04;

public class Day04 : Base
{
    public Day04()
    {
        Day = "4";
    }
    
    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        string puzzle = String.Join("", input);

        int numCols = input[0].Length, numRows = input.Length;
        int xmasCount = 0;
        
        ValueTuple<int, int>[] directions =
        [
            (1, 1),    // Diagonal down-right
            (1, -1),   // Diagonal down-left
            (-1, 1),   // Diagonal up-right
            (-1, -1),  // Diagonal up-left
            (-1, 0),   // up
            (1, 0),    // down
            (0, 1),     // right
            (0, -1)     // left
        ];
        Dictionary<string, string> test = new Dictionary<string, string>();
        foreach (var (letter, idx) in puzzle.Enumerate())
        {
            test.Clear();
            int column = idx % input.Length;
            int row = idx / input.Length;

            if (letter != 'X')
            {
                continue;
            }
            
            for (int mod = 1; mod <= 3; mod++)
            {
                foreach (var (dirRow, dirCol) in directions)
                {
                    int newRow = row + mod * dirRow;
                    int newCol = column + mod * dirCol;
                    string key = $"{dirRow}{dirCol}";

                    test.TryAdd(key, "X");
                    
                    if (newRow >= 0 && newRow < numRows && newCol >= 0 && newCol < numCols)
                    {
                        test[key] += puzzle[newRow * numCols + newCol];
                    }
                }
            }
            xmasCount += test.Values.Count(x => x == "XMAS" || x == "SAMX");
        }
        
        return xmasCount;
    }

    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        string puzzle = String.Join("", input);

        int numCols = input[0].Length, numRows = input.Length;
        int xmasCount = 0;
        
        ValueTuple<int, int>[] directions =
        [
            (1, 1),    // Diagonal down-right
            (1, -1),   // Diagonal down-left
            (-1, 1),   // Diagonal up-right
            (-1, -1),  // Diagonal up-left
        ];
        
        foreach (var (letter, idx) in puzzle.Enumerate())
        {
            int column = idx % input.Length;
            int row = idx / input.Length;
            
            if (letter != 'A')
            {
                continue;
            }
            
            // only one step in each direction 
            // if one is oob it cant be x-mas
            if (row + 1 >= numRows || column + 1 >= numCols || column - 1 < 0 || row - 1 < 0)
            {
                continue;
            }

            string test = "";
            foreach (var (dirRow, dirCol) in directions)
            {
                int newRow = row + dirRow;
                int newCol = column + dirCol;
                test += puzzle[newRow * numCols + newCol];
            }

            if (test is "MMSS" or "SSMM" or "MSMS" or "SMSM")
            {
                xmasCount++;
            }
        }
        return xmasCount;
    }
}