using System.Diagnostics;
using System;

namespace _2025._12
{
    
    public sealed class Day12 : Base
    {
        List<HashSet<(int, int)>> _presents = [];
        List<(int rows, int cols, int[] presents)> _christmasTrees = [];
        public Day12(bool example) : base(example)
        {
            Day = "12";
        }

        private void ParseInput()
        {
            string[] input = ReadInput();
            for(int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(input[i]))
                {
                    continue;
                }
                if(input[i].Contains("x"))
                {
                    string[] split = input[i].Split(": ");
                    int splitter = split[0].IndexOf("x", StringComparison.Ordinal);
                    _christmasTrees.Add((
                        int.Parse(split[0][(splitter+1)..]), 
                        int.Parse(split[0][..splitter]),
                        split[1].Split(" ").Select(int.Parse).ToArray())
                    );
                    continue;
                }

                if (input[i][^1] != ':')
                {
                    continue;
                }

                _presents.Add([]);
                i++;
                for(int presentStart = i; !string.IsNullOrWhiteSpace(input[i]); i++)
                {
                    _presents[^1].UnionWith(input[i]
                        .Select((x, index) => (x, index))
                        .Where(x => x.x == '#')
                        .Select(x => (i - presentStart, x.index))
                        .ToHashSet()
                    );
                } 
            }
        }

        private void PrintInput()
        {
            foreach ((var present, int index) in _presents.Select((x, i) => (x, i)))
            {
                Console.WriteLine($"{index}: ");
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (present.Contains((row, col)))
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                    
                }
                Console.WriteLine();
            }

            foreach (var christmasTree in _christmasTrees)
            {
                Console.WriteLine($"{christmasTree.rows}x{christmasTree.cols}: {string.Join(" ",  christmasTree.presents.Select(x => x.ToString()))}");
            }
        }
        
        public override object PartOne()
        {
            ParseInput();
            int numFitting = 0;
            foreach ((int rows, int cols, int[] presents) in _christmasTrees)
            {
                if (rows * cols < 9 * presents.Sum())
                {
                    Console.WriteLine($"Definitely not enough space! {presents.Sum()*9} {rows}x{cols}: {string.Join(" ", presents.Select(x => x.ToString()))}");
                    continue;
                }
                
                // TODO CHECK? 
                numFitting++;
            }
            
            return numFitting;
        }

        public override object PartTwo()
        {
            return "";
        }
    }
}
