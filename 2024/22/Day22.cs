
using System.Collections.Immutable;
using System.Data;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._22;

public struct Bidder(int totalAmountNumbers)
{
    public long[] SecretNumbers { get; set; } = new long[totalAmountNumbers];
    public int[] Prices { get; set; } = new int[totalAmountNumbers];
    public int[] Differences { get; set; } = new int[totalAmountNumbers];
}

public class Day22 : Base
{

    private readonly long[] _initialSecretNumbers;
    
    public Day22(bool example) : base(example)
    {
        Day = "22";
        _initialSecretNumbers = ReadInput().Select(long.Parse).ToArray();
    }

    private static long MixAndPrune(long toMix, long original)
    {
        original ^= toMix;
        original = MathExtensions.Modulo(original, 16777216);
        return original;
    }
    
    private static long EvolveSecretNumber(long number)
    {
        number = MixAndPrune(number * 64, number);
        number = MixAndPrune(number / 32, number);
        return MixAndPrune(number * 2048, number);
    }

    private Bidder[] _bidders = [];
    // in total we will have 2001 numbers, as we start with one and generate 2000
    private const int SecretNumbers = 2001;
    
    private long SolveProblem(int stage)
    {
        _bidders = new Bidder[_initialSecretNumbers.Length];
        foreach ((long number, int idx) in _initialSecretNumbers.Enumerate())
        {
            _bidders[idx] = new Bidder(SecretNumbers)
            {
                SecretNumbers =
                {
                    [0] = number
                },
                Differences =
                {
                    [0] = 0
                },
                Prices =
                {
                    [0] = (int)(number % 10)
                }
            };
        }
        
        for (int it = 1; it < SecretNumbers; it++)
        {
            foreach(Bidder bidder in _bidders)
            {
                bidder.SecretNumbers[it] = EvolveSecretNumber(bidder.SecretNumbers[it - 1]);
                bidder.Prices[it] = (int)(bidder.SecretNumbers[it] % 10);
                bidder.Differences[it] = bidder.Prices[it] - bidder.Prices[it-1];
            }
        }

        if (stage == 1)
        {
            return _bidders.Select(x => x.SecretNumbers.Last()).Sum();
        }

        Dictionary<ValueTuple<int, int, int, int>, int> prices = [];
        // make sure to add each price difference range only once per seller
        HashSet<ValueTuple<int, int, int, int>> seenRanges = [];
        foreach (Bidder bidder in _bidders)
        {
            for (int it = 1; it < SecretNumbers - 3; it++)
            {
                ValueTuple<int, int, int, int> range = (bidder.Differences[it], bidder.Differences[it + 1],
                    bidder.Differences[it + 2], bidder.Differences[it + 3]);
                if (!seenRanges.Add(range))
                {
                    continue;
                }

                prices.TryAdd(range, 0);
                prices[range] += bidder.Prices[it+3];
            }
            seenRanges.Clear();
        }
        
        return prices.Values.Max();
    }
    
    public override object PartOne()
    {
        return SolveProblem(1);
    }
    
    public override object PartTwo()
    {
        return SolveProblem(2);
    }

    public override void Reset()
    {
        _bidders = [];
    }
}

