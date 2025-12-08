using System.Collections.Immutable;

namespace _2025._08
{
    public sealed class Day08 : Base
    {
        
        
        public Day08(bool example) : base(example)
        {
            Day = "08";
        }

        private static double GetDistance((int X, int Y, int Z) a, (int X, int Y, int Z) b)
        {
            return Math.Sqrt(
                Math.Pow(a.X - b.X, 2) + 
                Math.Pow(a.Y - b.Y, 2) + 
                Math.Pow(a.Z - b.Z, 2)
            );
        }

        private object SolvePuzzle(int part)
        {

            List<HashSet<(int, int, int)>> circuits = [];
            var input = ReadInput()
                .Select(x =>{
                    var split = x.Split(',').Select(int.Parse).ToArray();
                    return (X: split[0], Y: split[1], Z: split[2]);
                })
                .ToImmutableArray();
            
            var junctionBoxDistances = input
                    .Enumerate()
                    .SelectMany(junctionBox => 
                        input
                            .Skip(junctionBox.index+1)
                            .Select(junctionBox2 => 
                                (
                                    Distance: GetDistance(junctionBox.item, junctionBox2), 
                                    JunctionBox: junctionBox.item, 
                                    JunctionBox2: junctionBox2
                                )
                            )
                        )
                    .ToImmutableSortedSet()
                    .Take(part == 1 ? (Example ? 10 : 1000) : int.MaxValue);
        
            foreach(var currConnection in junctionBoxDistances)
            {
                var filtered = circuits
                    .Select((circuit, index) => new { circuit, index })
                    .Where(x => 
                        x.circuit.Any(y => y == currConnection.JunctionBox || y == currConnection.JunctionBox2 )
                    )
                    .OrderByDescending(x => x.index)
                    .ToList();
                if (filtered.Count == 0)
                {
                    circuits.Add([currConnection.JunctionBox, currConnection.JunctionBox2]);
                    continue;
                }
                filtered[0].circuit.Add(currConnection.JunctionBox);
                filtered[0].circuit.Add(currConnection.JunctionBox2);

                for(int i = filtered.Count - 1; i >= 1; i--)
                {
                    filtered[0].circuit.UnionWith(filtered[i].circuit);
                    circuits.RemoveAt(filtered[i].index);
                }

                if (part == 1)
                {
                    continue;
                }

                if (filtered[0].circuit.Count == input.Length)
                {
                    return currConnection.JunctionBox.X * currConnection.JunctionBox2.X;
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
