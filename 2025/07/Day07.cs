using System.Diagnostics;
using System;

namespace _2025._07
{
    public struct CacheEntry
    {
        public ulong PossiblePaths;
        public bool HitSplitter; // used for stage 1
    }
    
    public sealed class Day07 : Base
    {
        private string[] _TachyonManifold;
        private Dictionary<(int, int), CacheEntry> _cache = [];
        
        public Day07(bool example) : base(example)
        {
            Day = "07";
        }

        private ulong SimulateTachyonBeam((int, int) pos)
        {
            while (true)
            {
                if (_cache.TryGetValue(pos, out CacheEntry cacheEntry))
                {
                    return cacheEntry.PossiblePaths;
                }

                // go straight down
                (int, int) newPos = (pos.Item1 + 1, pos.Item2);

                if (!_TachyonManifold.TryGetValue(newPos, out char? field))
                {
                    _cache.Add(pos, new CacheEntry
                    {
                        PossiblePaths = 1,
                        HitSplitter = false
                    });
                    return 1;
                }

                if (field == '^')
                {
                    ulong possiblePaths = SimulateTachyonBeam((newPos.Item1, newPos.Item2 - 1)) +
                                          SimulateTachyonBeam((newPos.Item1, newPos.Item2 + 1));
                    _cache.Add(pos, new CacheEntry
                    {
                        PossiblePaths = possiblePaths,
                        HitSplitter = true
                    });
                    return possiblePaths;
                }
                pos = newPos;
            }
        }

        private (int, int) PrepareInput()
        {
            _TachyonManifold = ReadInput();
            _cache = []; 
            for (int row = 0; row < _TachyonManifold.Length; row++)
            {
                for (int col = 0; col < _TachyonManifold[row].Length; col++)
                {
                    if (_TachyonManifold[row][col] == 'S')
                    {
                        return (row, col);
                    }
                }
            }
            return (0, 0);
        }
        
        public override object PartOne()
        {
            SimulateTachyonBeam(PrepareInput());
            return _cache
                .Where(x => x.Value.HitSplitter)
                .Select(x => x.Key)
                .ToHashSet()
                .Count;
        }

        public override object PartTwo()
        {
            return SimulateTachyonBeam(PrepareInput());
        }
    }
}
