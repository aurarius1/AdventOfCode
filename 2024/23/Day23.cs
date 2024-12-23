
using System.Collections.Immutable;
using System.Data;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._23;

public class Day23 : Base
{
    private readonly Dictionary<string, HashSet<string>> _computers = [];
    private readonly HashSet<HashSet<string>> _cliques = [];
    
    public Day23(bool example) : base(example)
    {
        Day = "23";
        string[] computerPairs = ReadInput();
        
        // build adjacency list for graph
        foreach (string computerPair in computerPairs)
        {
            string[] pair = computerPair.Split('-');
            _computers.TryAdd(pair[0], []);
            _computers.TryAdd(pair[1], []);
            
            _computers[pair[0]].Add(pair[1]);
            _computers[pair[1]].Add(pair[0]);
        }
    }
    
    private void BronKerbosch(HashSet<string> r, HashSet<string> p, HashSet<string> x, int stage)
    {
        switch (stage)
        {
            case 1 when r.Count == 3:
            case 2 when p.Count == 0 && x.Count == 0:
                _cliques.Add([..r]);
                return;
        }

        foreach (string node in new HashSet<string>(p))
        {
            // build new tmp sets, to avoid adding to the original set
            HashSet<string> pTmp = [..p], xTmp = [..x], rTmp = [..r, node];
            pTmp.IntersectWith(_computers[node]);
            xTmp.IntersectWith(_computers[node]);
            
            BronKerbosch(rTmp, pTmp, xTmp, stage);
            
            p.Remove(node);
            x.Add(node);
        }
    }
    
    public override object PartOne()
    {
        BronKerbosch([], _computers.Keys.ToHashSet(), [], 1);
        IEnumerable<HashSet<string>> filteredCliques = _cliques.Where(x => x.Any(y => y.StartsWith('t')));
        return filteredCliques.Count();
    }
    
    public override object PartTwo()
    {
        BronKerbosch([], _computers.Keys.ToHashSet(), [], 2);
        HashSet<string> maxClique = _cliques.OrderByDescending(x => x.Count).First();
        return string.Join(",", maxClique.Order());
    }

    public override void Reset()
    {
    }
}

