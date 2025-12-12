using System.Diagnostics;
using System;
using System.Collections.Immutable;

namespace _2025._11
{
    public sealed class Day11 : Base
    {
        private Dictionary<string, string[]> _devices = [];
        private Dictionary<(string, bool, bool), long> _cache = [];
        public Day11(bool example) : base(example)
        {
            Day = "11";
        }

        public override void Reset()
        {
            _cache.Clear();
        }
        
        private long FindAllPaths(string curr, bool visitedDac = true, bool visitedFft = true)
        {
            (string, bool, bool) key = (curr, visitedDac, visitedFft);
            if (_cache.TryGetValue(key, out long visitedPaths))
            {
                return visitedPaths;
            }
            if (curr == "out")
            {
                _cache[key] = visitedDac && visitedFft ? 1L : 0L;
                return _cache[key];
            }
            _cache[key] = _devices[curr]
                .Sum(output => output switch
                {
                    "dac" => FindAllPaths(output, true, visitedFft),
                    "fft" => FindAllPaths(output, visitedDac, true),
                    _ => FindAllPaths(output, visitedDac, visitedFft)
                });
            return _cache[key];

        }
        
        public override object PartOne()
        {
            _devices = ReadInput()
                .Select(x =>x.Split(' '))
                .ToDictionary(x => x[0][..^1], x => x[1..]);
            return FindAllPaths("you");
        }

        public override object PartTwo()
        {
            _devices = (Example ? File.ReadAllLines(Path.Combine(ClassPath, "example_stage2")) : ReadInput())
                .Select(x =>x.Split(' '))
                .ToDictionary(x => x[0][..^1], x => x[1..]);
            return FindAllPaths("svr", false, false);
        }
    }
}
