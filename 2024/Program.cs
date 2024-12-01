using _2024.Utils;

namespace _2024
{
    class Program
    {
        static void ParseArguments(string[] args, out Stages stage, out bool example, out string day)
        {
            day = args[0].PadLeft(2, '0'); // Ensure the day number is two digits, e.g., "01"
            example = false;
            stage = Stages.All;
            bool part1 = false, part2 = false;
            
            foreach(string arg in args.Skip(1))
            {
                example |= (arg == "--example" || arg == "-e");
                part2 |= arg == "--part2";
                part1 |= arg == "--part1";
            }
            
            // executing all is default
            if ((!part1 && !part2) || (part1 && part2))
            {
                return;
            }

            if (part1)
            {
                stage = Stages.One;
                return;
            }

            if (part2)
            {
                stage = Stages.Two;
                return;
            }
        }
        
        
        static void Main(string[] args)
        {
            // Check if a day number was passed as an argument
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the day number as an argument (e.g., 01 for Day01).");
                System.Environment.Exit(-1);
            }
            ParseArguments(args, out Stages stage, out bool example, out string dayNumber);

            var className = $"_2024._{dayNumber}.Day{dayNumber}";
            Type? dayType = null;
            try
            {
                dayType = Type.GetType(className);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                System.Environment.Exit(-1);
            }
            
            if (dayType == null || !typeof(Base).IsAssignableFrom(dayType))
            {
                Console.WriteLine($"Class {className} not found. Make sure the namespace and class name match.");
                System.Environment.Exit(-1);
            }
            
            var day = (Base?)Activator.CreateInstance(dayType);
            if (day == null)
            {
                Console.WriteLine($"Unable to create an instance of {className}.");
                System.Environment.Exit(-1);
            }

            day.Solve(example, stage);
        }
    }
}
