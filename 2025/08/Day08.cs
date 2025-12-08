using System.Diagnostics;
using System;

namespace _2025._08
{
   

    public sealed class Day08 : Base
    {
        
        
        public Day08(bool example) : base(example)
        {
            Day = "08";
        }

        private static double GetDistance((int, int, int) a, (int, int, int) b)
        {
            return Math.Sqrt(
                Math.Pow(a.Item1 - b.Item1, 2) + 
                Math.Pow(a.Item2 - b.Item2, 2) + 
                Math.Pow(a.Item3 - b.Item3, 2)
            );
        }

        private object SolvePuzzle(int part)
        {
            SortedSet<(double, (int, int, int), (int, int, int))> junctionBoxDistances = [];
            List<HashSet<(int, int, int)>> circuits = [];
            List<(int, int, int)> input = ReadInput().Select(x =>
            {
                var split = x.Split(',').Select(y => int.Parse(y)).ToArray();
                return (split[0], split[1], split[2]);
            }).ToList();

            foreach (var (junctionBox, index) in input.Select((x, i) => (x, i)))
            {
                foreach (var junctionBox2 in input.Skip(index+1))
                {
                    double distance = GetDistance(junctionBox, junctionBox2);
                    junctionBoxDistances.Add((distance, junctionBox, junctionBox2));
                }
            }

            var range = part == 1 ? (junctionBoxDistances.Take(Example ? 10 : 1000)) : (junctionBoxDistances);
            foreach(var currConnection in range)
            {
                var filtered = circuits
                    .Select((circuit, index) => new { circuit, index })
                    .Where(x => 
                        x.circuit.Any(y => y == currConnection.Item2 || y == currConnection.Item3 )
                    )
                    .OrderByDescending(x => x.index)
                    .ToList();
                if (filtered.Count == 0)
                {
                    circuits.Add( [currConnection.Item2, currConnection.Item3 ]);
                    continue;
                }
                filtered[0].circuit.Add(currConnection.Item2);
                filtered[0].circuit.Add(currConnection.Item3);

                for(int i = filtered.Count - 1; i >= 1; i--)
                {
                    filtered[0].circuit.UnionWith(filtered[i].circuit);
                    circuits.RemoveAt(filtered[i].index);
                }

                if (part == 1)
                {
                    continue;
                }

                if (filtered[0].circuit.Count == input.Count)
                {
                    return currConnection.Item2.Item1 * currConnection.Item3.Item1;
                }
            }

            return circuits
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .Take(3)
                .Aggregate((x, y) => x * y);
        }

        public override object PartOne()
        {
            return SolvePuzzle(1);
        }

        public override object PartTwo()
        {
            return SolvePuzzle(2);
        }
    }
}
