
using System.Diagnostics;
using _2024.Utils;


namespace _2024._01;

public sealed class Day01 : Base
{
    
    public Day01()
    {
        Day = "01";
    }

    private void ParseInput(string[] input, out List<int> firstList, out List<int> secondList)
    {
        firstList = new List<int>();
        secondList = new List<int>();
        

        foreach(string line in input)
        {
            string[] ids = line.Split("   ");
            
            bool valid = int.TryParse(ids[0], out int first);
            valid &= int.TryParse(ids[1], out int second);

            if (!valid)
            {
                throw new Exception($"Invalid input: {line}");
            }

            firstList.Add(first);
            secondList.Add(second);
        }
    }
    
    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        ParseInput(input, out List<int> firstIdList, out List<int> secondIdList);
        Debug.Assert(firstIdList.Count == secondIdList.Count);

        int distances = 0;
        while (firstIdList.Count > 0)
        {
            int firstMin = firstIdList.IndexOfMin();
            int secondMin = secondIdList.IndexOfMin();
            distances += Math.Abs(firstIdList[firstMin] - secondIdList[secondMin]);
            firstIdList.RemoveAt(firstMin);
            secondIdList.RemoveAt(secondMin);
        }
        
        return distances.ToString();
    }

    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        ParseInput(input, out List<int> firstIdList, out List<int> secondIdList);
        Debug.Assert(firstIdList.Count == secondIdList.Count);
        
        var occurrences = secondIdList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        int distances = 0;
        foreach (int id in firstIdList)
        {
            distances += id * occurrences.GetValueOrDefault(id, 0);
        }
        
        return distances.ToString();
    }
}