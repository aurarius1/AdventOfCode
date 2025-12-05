using System.Diagnostics;
using System;

namespace _2025._05
{
    public sealed class Day05 : Base
    {
        public Day05(bool example) : base(example)
        {
            Day = "05";
        }

        private (List<(ulong, ulong)>, IEnumerable<ulong> ingredients) ParseInput()
        {
            string[] input = ReadInput();
            var ranges = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(x =>
            {
                var span = x.AsSpan();
                int dash = span.IndexOf('-');
                return (ulong.Parse(span[..dash]), ulong.Parse(span[(dash+1)..]));
            }).OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList();
            
            for (int currRange = 0; currRange < ranges.Count; currRange++)
            {
                var (lower, upper) = ranges[currRange];
                int nextRange = currRange + 1;
                while(nextRange < ranges.Count)
                {
                    (ulong nextLower, ulong nextUpper) = ranges[nextRange];
                    if (nextLower >= ranges[currRange].Item1 && nextLower <= ranges[currRange].Item2)
                    {
                        ranges[currRange] = (ranges[currRange].Item1, Math.Max(ranges[currRange].Item2, nextUpper));
                        ranges.RemoveAt(nextRange);
                        continue;
                    }
                    nextRange++;
                }
            }

            var ingredients = input
                .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
                .Skip(1)
                .Select(ulong.Parse);
            return (ranges, ingredients);
        }
        
        
        public override object PartOne()
        {
            var (ranges, ingredients) = ParseInput();
            return ingredients.Count(ingredient => 
                ranges.Any(r => ingredient >= r.Item1 && ingredient <= r.Item2)
            );
        }

        public override object PartTwo()
        {
            var (ranges, _) = ParseInput();
            return ranges.Aggregate< (ulong, ulong), ulong >(
                0, (acc, x) =>  acc + (x.Item2 - x.Item1 +1)
            );;
        }
    }
}
