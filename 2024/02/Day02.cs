using _2024.Utils;

namespace _2024._02;

public class Day02 : Base
{
    public Day02()
    {
        Day = "02";
    }

    private void ParseLine(string line, out List<int> readings)
    {
        readings = new();
        string[] parts = line.Split();
        
        for (int i = 0; i < parts.Length; i++)
        {
            int parsed = int.Parse(parts[i]);
            readings.Add(parsed);
        }

    }

    private bool IsValidDiff(int reading1, int reading2)
    {
        int diff = Math.Abs(reading1 - reading2);
        return diff is >= 1 and <= 3;
    }

    private int ReportSafe(List<int> readings)
    {
        for (int i = 1; i < readings.Count-1; i++)
        {
            bool directionSame = (readings[i - 1] < readings[i]) == (readings[i] < readings[i + 1]);
            bool diffsValid = IsValidDiff(readings[i-1], readings[i]) && IsValidDiff(readings[i+1], readings[i]);
            if (!directionSame || !diffsValid)
            {
                return 0;
            }
        }
        return 1;
    }
    
    public override string PartOne(bool example)
    {
        string[] input = ReadInput(example);
        int safeReadings = 0;
        foreach(var line in input)
        {
            ParseLine(line, out var readings);
            safeReadings += ReportSafe(readings);
        }
        return safeReadings.ToString();
    }

    public override string PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        int safeReadings = 0;
        foreach(var line in input)
        {
            ParseLine(line, out var readings);
            for(int i = 0; i < readings.Count; i++) {
                List<int> tmp = readings.Where((_, idx) => idx != i).ToList();
                if (ReportSafe(tmp) == 1)
                {
                    safeReadings++;
                    break;
                }
            }
        }
        return safeReadings.ToString();
    }
}