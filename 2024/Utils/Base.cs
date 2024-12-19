namespace _2024.Utils;

using System.Diagnostics;

public abstract class Base
{
    private string _day = "";
    protected string Day
    {
        get { return _day;}
        init { _day = value.PadLeft(2, '0'); } 
    }

    protected string ClassPath
    {
        get
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(currentDirectory, Day);
        }
    }

    protected string ExampleData
    {
        get { return Path.Combine(ClassPath, "example"); }
    }

    protected string RealData
    {
        get { return Path.Combine(ClassPath, "input"); }
    }
    
    public abstract object PartOne(bool example);
    public abstract object PartTwo(bool example);

    public virtual void Reset()
    {
        
    }
    
    protected virtual string[] ReadInput(bool example)
    {
        string path = RealData;
        if (example)
        {
            path = ExampleData;
        }

        bool exists = File.Exists(path);
        if (!exists)
        {
            throw new FileNotFoundException($"The file {path} was not found.");
        }

        string[] lines = File.ReadAllLines(path);
        return lines;
    }
}

public static class BaseExtensions
{
    public static void Solve(this Base problem, bool example, List<Stages> stages)
    {
        List<ValueTuple<Stages, string, double, long>> results = [];
        int maxLength = 0;
        foreach (Stages stage in stages)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            string solution = stage switch
            {
                Stages.One => problem.PartOne(example).ToString() ?? "",
                Stages.Two => problem.PartTwo(example).ToString() ?? "",
                _ => throw new Exception($"Unknown stage {stage}")
            };
            stopwatch.Stop();
            results.Add((stage, solution, stopwatch.Elapsed.TotalSeconds, stopwatch.ElapsedMilliseconds));
            maxLength = Math.Max(maxLength, results[^1].Item2.Length);
            problem.Reset();

        }
        foreach ((Stages stage, string solution, double seconds, long milliSeconds) in results)
        {
            Console.WriteLine($"Stage {stage}: {solution.PadLeft(maxLength, ' ')} " +
                              $"took {seconds:F2}s ({milliSeconds,4}ms)");
        }
    }
    
}