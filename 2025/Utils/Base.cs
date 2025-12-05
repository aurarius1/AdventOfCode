namespace _2025.Utils;

using System.Diagnostics;

public abstract class Base(bool example)
{
    protected readonly bool Example = example; 
    
    private readonly string _day = "";
    protected string Day
    {
        get => _day;
        init => _day = value.PadLeft(2, '0');
    }

    protected string ClassPath
    {
        get
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(currentDirectory, Day);
        }
    }

    private string ExampleData => Path.Combine(ClassPath, "example");
    private string RealData => Path.Combine(ClassPath, "input");

    public abstract object PartOne();
    public abstract object PartTwo();

    public virtual void Reset()
    {
        
    }
    
    protected string[] ReadInput()
    {
        string path = RealData;
        if (Example)
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
    public static void Solve(this Base problem, List<Stages> stages)
    {
        List<ValueTuple<Stages, string, double, long>> results = [];
        int maxLength = 0;
        foreach (Stages stage in stages)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            string solution = stage switch
            {
                Stages.One => problem.PartOne().ToString() ?? "",
                Stages.Two => problem.PartTwo().ToString() ?? "",
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