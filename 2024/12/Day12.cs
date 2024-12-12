using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._12;

public class Day12 : Base
{
    public Day12()
    {
        Day = "12";
    }

    private string[] _input = [];
    private readonly HashSet<ValueTuple<int, int>> _areaPositions = [];

    private void FindArea(int row, int col, char soil, HashSet<ValueTuple<int, int>> visited)
    {
        if (!visited.Add((row, col)))
        {
            return;
        }
        if (_input[row][col] == soil)
        {
            _areaPositions.Add((row, col));
        }
        foreach ((int dRow, int dCol) in  new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
        {
            (int newRow, int newCol) = (row+dRow, col+dCol);

            if (_input.IsOob((newRow, newCol)))
            {
                continue;
            }

            if (_input[newRow][newCol] != soil)
            {
                continue;
            }
            FindArea(newRow, newCol, soil, visited);
        }
    }
    
    private int GetPerimeter(char soil, int stage)
    {
        int perimeter = 0;
        foreach ((int row, int col) in _areaPositions)
        {
            int adjacentSides = 0;
            // count how many adjacent fields are of the same soil type
            foreach ((int dRow, int dCol) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
            {
                int newRow = row + dRow, newCol = col + dCol;
                if (_input.IsOob((newRow, newCol)) || _input[newRow][newCol] != soil)
                {
                    if (stage == 1)
                    {
                        perimeter++;
                    }
                    
                    continue;
                }

                if (_input[newRow][newCol] == soil)
                {
                    adjacentSides++;
                }
            }

            // stage 1 doesnt count sides, but actual perimeter
            if (stage == 1)
            {
                continue;
            }
            // count corners
            switch (adjacentSides)
            {
                case 0:
                    // handles single "island" case 
                    perimeter += 4;
                    continue;
                case 1:
                    // such a case: aa 
                    perimeter += 2;
                    continue;
            }
            
            // checks these four shapes: 
            // aa   b     c   cc
            // a    bb   cc    c
            // any of those would signal one corner
            foreach((int dRow, int dCol) in new [] {(-1, 1), (-1, -1), (1, 1), (1, -1)}){
                 int newRow = row + dRow, newCol = col + dCol;
                
                // this is not a valid corner
                if (_input.IsOob((newRow, newCol)) || _input[row][newCol] != soil || _input[newRow][col] != soil)
                {
                    continue;
                }
                
                // both positions need to be either oob or not equal to the current soil
                // otherwise this is might not be a valid corner
                (int, int) testPos1 = (row + (dRow * -1), col);
                (int, int) testPos2 = (row, col + (dCol * -1));
                if ((!_input.TryGetValue(testPos1, out char? pos1) || pos1 != soil) && 
                    (!_input.TryGetValue(testPos2, out char? pos2) || pos2 != soil))
                {
                    perimeter++;
                }
                // this position must not be equal to the current soil 
                // otherwise now this is definitely not a corner
                (int, int) testPos3 = (row + dRow, col + dCol);
                if (_input.TryGetValue(testPos3, out char? pos3) && pos3 != soil)
                {
                    perimeter += 1;
                }
    
            }
        }
        return perimeter; 
    }

    private int SolvePuzzle(bool example, int stage)
    {
        _input = ReadInput(example);
        List<ValueTuple<char, int, int>> farmfields = [];
        HashSet<ValueTuple<int, int>> visitedAreas = [];
        
        foreach ((string line, int row) in _input.Enumerate())
        {
            foreach ((char soil, int col) in line.Enumerate())
            {
                if (!visitedAreas.Add((row, col)))
                {
                    continue;
                }

                FindArea(row, col, soil, []);
                farmfields.Add((soil, _areaPositions.Count, GetPerimeter(soil, stage)));
                visitedAreas.UnionWith(_areaPositions);
                _areaPositions.Clear();
            }
        }

        int price = 0;
        foreach ((char soil, int area, int perimeter) in farmfields)
        {
            price += (area*perimeter);
        }
        return price;
    }
    
    public override object PartOne(bool example)
    {
        return SolvePuzzle(example, 1);

    }

    public override object PartTwo(bool example)
    {
        return SolvePuzzle(example, 2);
    }

    public override void Reset()
    {
        _areaPositions.Clear();
        _input = [];
    }
}