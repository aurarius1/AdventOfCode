namespace _2024.Utils;

using System.Diagnostics;

public abstract class Base
{
    protected string Day { get; init; } = string.Empty;

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
    public static void Solve(this Base problem, bool example, Stages stage = Stages.All)
    {
        if (stage == Stages.All)
        {
            problem.Solve(example, Stages.One);
            problem.Solve(example, Stages.Two);
            return;
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        object solution;
        switch (stage)
        {
            case Stages.One: 
                solution = problem.PartOne(example);
                break;
            case Stages.Two:
                solution = problem.PartTwo(example);
                break;
            default: 
                throw new Exception($"Unknown stage {stage}");
        }
        stopwatch.Stop();
        double seconds = stopwatch.Elapsed.TotalSeconds;
        Console.WriteLine($"Stage {stage}: {solution} took {seconds:F2}");
    }
    
}