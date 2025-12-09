using System.Diagnostics;
using System;
using System.Collections.Immutable;

namespace _2025._09
{
    
    public sealed class Day09 : Base
    {
        private HashSet<(long X, long Y)> _wallTiles = [];
        Dictionary<((long, long), (long, long)), long> _areas = [];
        
        public Day09(bool example) : base(example)
        {
            Day = "09";
        }

        private static long GetArea((long X, long Y) tile, (long X, long Y) tile2)
        {
            return (Math.Abs(tile2.X - tile.X) + 1) * (Math.Abs(tile2.Y - tile.Y) + 1);
        }
        
        public override object PartOne()
        {
            var redTiles = ReadInput().Select(x =>
            {
                var split = x.Split(',').Select(long.Parse).ToImmutableArray();
                return (X: split[0], Y: split[1]);
            }).ToArray();
            return redTiles
                .Select((x, i) => (tile: x, index: i))
                .Aggregate<((long X, long Y) tile, int index), long>(0, (current, redTile) => 
                    redTiles
                        .Skip(redTile.index + 1)
                        .Select(redTile2 => GetArea(redTile2, redTile.tile))
                        .Prepend(current)
                        .Max()
                    );
        }

        private HashSet<(long X, long Y)> FloodFill((long X, long Y) start)
        {
            HashSet<(long X, long Y)> insideTiles = [.._wallTiles];
            Queue<(long X, long Y)> toInspect = new();
            toInspect.Enqueue((start.X, start.Y));
            (long X, long Y)[] neighbours = [(+1, 0), (0, +1), (-1, 0), (0, -1)];
            while (toInspect.Count > 0)
            {
                var tile = toInspect.Dequeue();
                if (!insideTiles.Add(tile))
                {
                    continue;
                }

                foreach (var neighbour in neighbours)
                {
                    (long X, long Y) newTile = (tile.X + neighbour.X, tile.Y + neighbour.Y);
                    if (_wallTiles.Contains(newTile) || insideTiles.Contains(newTile))
                    {
                        continue;
                    }
                    
                    toInspect.Enqueue(newTile);
                }
            }
            return insideTiles;
        }

        private void ParseWall((long X, long Y)[] redTiles)
        {
            foreach (var curr in redTiles.Select((x, i) => (tile: x, index: i)))
            {
                int startIdx = curr.index == 0 ? redTiles.Length - 1 : curr.index - 1;
                (long X, long Y) start = redTiles[startIdx];
                (long X, long Y) move = curr.tile.X == start.X ? 
                    (0, (curr.tile.Y - start.Y) / Math.Abs((curr.tile.Y - start.Y))) : 
                    ((curr.tile.X - start.X) / Math.Abs((curr.tile.X - start.X)), 0);
                while (start != curr.tile)
                {
                    start = (start.X + move.X, start.Y + move.Y);
                    _wallTiles.Add((start.X, start.Y));
                }
            }
        }

        private (long X, long Y)[] ReduceCoordinates((long X, long Y)[] redTiles)
        {
            var reducedXCoordinates = redTiles.Select(x => x.X).Distinct().OrderBy(x => x).Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);
            var reducedYCoordinates = redTiles.Select(x => x.Y).Distinct().OrderBy(x => x).Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);

            
            foreach (var curr in redTiles.Select((x, i) => (tile: x, index: i)))
            {
                var reduced = (reducedXCoordinates[curr.tile.X], reducedYCoordinates[curr.tile.Y]);
                foreach (var secondTile in redTiles.Skip(curr.index + 1))
                {
                    var reduced2 = (reducedXCoordinates[secondTile.X], reducedYCoordinates[secondTile.Y]);
                    _areas[(reduced, reduced2)] = GetArea(curr.tile, secondTile);
                }

                redTiles[curr.index] = reduced;
            }

            return redTiles;
        }

        private static (long X, long Y) GetElementInside((long X, long Y)[] redTiles)
        {
            var offset = ((redTiles[1].X - redTiles[0].X), (redTiles[1].Y - redTiles[0].Y)) switch
            {
                (> 0, _) => (1, -1),
                (< 0, _) => (-1, 1),
                (_, > 0) => (-1, 1),
                (_, < 0) => (1, 1), 
                _ => (0, 0)
            };
            return (redTiles[0].X + offset.Item1, redTiles[0].Y + offset.Item2);
        }

        public override object PartTwo()
        {
            var redTiles = ReadInput().Select(x =>
            {
                var split = x.Split(',').Select(long.Parse).ToImmutableArray();
                return (X: split[0], Y: split[1]);
            }).ToArray();
            
            
            redTiles = ReduceCoordinates(redTiles);
            ParseWall(redTiles);


            (long X, long Y) start = GetElementInside(redTiles);
            HashSet<(long X, long Y)> inside = FloodFill(start);
            

            long maxArea = 0;
            foreach (var area in _areas)
            {
                long minCol = Math.Min(area.Key.Item1.Item2, area.Key.Item2.Item2);
                long maxCol =  Math.Max(area.Key.Item1.Item2, area.Key.Item2.Item2);
                long minRow = Math.Min(area.Key.Item1.Item1, area.Key.Item2.Item1);
                long maxRow = Math.Max(area.Key.Item1.Item1, area.Key.Item2.Item1);
                bool allInside = true;
                for (long col = minCol; col <= maxCol && allInside; col++)
                {
                    if (!inside.Contains((minRow, col)) || !inside.Contains((maxRow, col)))
                    {
                        allInside = false;
                    }
                }

                for (long row = minRow; row <= maxRow && allInside; row++)
                {
                    if (!inside.Contains((row, minCol)) || !inside.Contains((row, maxCol)))
                    {
                        allInside = false;
                    }
                }

                if (allInside)
                {
                    maxArea = Math.Max(maxArea, area.Value);
                }
               
                
            }
            
            return maxArea;
        }
    }
}
