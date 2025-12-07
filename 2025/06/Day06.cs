using System.Diagnostics;
using System;

namespace _2025._06
{
    public sealed class Day06 : Base
    {
        public Day06(bool example) : base(example)
        {
            Day = "06";
        }

        private IEnumerable<string[]> ParseInput()
        {
            var input = ReadInput();
            List<int> numberSizes = input[^1]
                .Select((op, idx) => (op, idx))
                .Where(x => x.op == '*' || x.op == '+')
                .Select(x => x.idx)
                .Append(input[^1].Length+1)
                .ToList();
            
            List<List<string>> split = input
                .SkipLast(1)
                .Select(line => numberSizes
                    .Zip(numberSizes.Skip(1))
                    .Select(indices => line[indices.First..(indices.Second-1)])
                    .ToList()
                )
                .Append(input[^1].Split(" ",  StringSplitOptions.RemoveEmptyEntries).ToList())
                .ToList();
           return Enumerable.Range(0, split.First().Count)
                .Select(c => split
                    .Select(row => row[c])
                    .ToArray()
                );
        }

        private ulong Accumulate(string op, ulong op1, ulong op2)
        {
            return op switch
            {
                "*" => op1 * op2,
                "+" => op1 + op2,
                _ => throw new ArgumentException($"Unknown operation: {op}")
            };
        }
        
        public override object PartOne()
        {
            var result = ParseInput().Select(row => row
                .SkipLast(1)
                .Select(x =>ulong.Parse(x))
                .Aggregate((x, y) => Accumulate(row[^1], x, y))
            ).Aggregate((x, y) => x + y);
            return result;
        }

        public override object PartTwo()
        {
            var result = ParseInput().Select(exercise =>
                Enumerable.Range(0, exercise.First().Length)
                    .Select(c =>
                        new string(
                            exercise
                                .SkipLast(1)
                                .Select(row => row[c])
                                .ToArray()
                        )
                    )
                    .Select(x => ulong.Parse(x))
                    .Aggregate((x, y) => Accumulate(exercise[^1], x, y))
            ).Aggregate((x, y) => x + y);
            return result;
        }
    }
}
