global using _2025.Utils;
using System.Reflection;

namespace _2025
{
    class Program
    {
        static void ParseArguments(string[] args, out List<Stages> stages, out bool example, out string day)
        {
            day = args[0].PadLeft(2, '0'); // Ensure the day number is two digits, e.g., "01"
            example = false;
            stages = [Stages.One, Stages.Two];
            bool part1 = false, part2 = false;
            
            foreach(string arg in args.Skip(1))
            {
                example |= (arg == "--example" || arg == "-e");
                part2 |= arg == "--part2";
                part1 |= arg == "--part1";
            }

            if (!part1 && !part2)
            {
                return;
            }
            stages = stages.Where(stage => 
                (part1 || stage != Stages.One) && 
                (part2 || stage != Stages.Two)).ToList();
        }

        static void RunDay(string dayNumber, List<Stages> stages, bool example)
        {
            string className = $"_2025._{dayNumber}.Day{dayNumber}";
            Type? dayType = null;
            try
            {
                dayType = Type.GetType(className);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Class {className} could not be found, maybe it wasn't solved yet!");
                return;
            }
            if (dayType == null || !typeof(Base).IsAssignableFrom(dayType))
            {
                Console.WriteLine($"Class {className} could not be found, maybe it wasn't solved yet!");
                return;
            }
            ConstructorInfo? constructor = dayType.GetConstructor([typeof(bool)]);
            if (constructor == null)
            {
                throw new InvalidProgramException();
            }
            
            Base? day = (Base?)constructor.Invoke([example]);
            if (day == null)
            {
                throw new InvalidProgramException($"Unable to create an instance of {className}.");
            }

            day.Solve(stages);
        }
        
        static void Main(string[] args)
        {
            // Check if a day number was passed as an argument
            List<Stages> stages = [Stages.One, Stages.Two];
            bool example = false;
            if (args.Length > 0)
            {
                ParseArguments(args, out stages, out example, out string dayNumber);
                RunDay(dayNumber, stages, example);
                return;
            }
            
            for (int i = 1; i <= 12; i++)
            {
                string day = i.ToString().PadLeft(2, '0');
                Console.WriteLine($"Running day {day}:");
                RunDay(day, stages, example);
                Console.WriteLine();
            }
        }
    }
}