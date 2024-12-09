using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using _2024.Utils;
using System.Threading.Tasks;

namespace _2024._09;

internal class FsBlock
{
    public FsBlock()
    {
    }

    public bool IsEmpty { get; set; } = false;
    public ulong? Id { get; set; } = null;
    public ulong Size { get; set; } = 0;
}

public class Day09 : Base
{
    public Day09()
    {
        Day = "9";
    }
    
    public override object PartOne(bool example)
    {
        List<ValueTuple<bool, ulong?>> fs = [];
        string[] input = ReadInput(example);
        ulong fileId = 0;
        foreach ((char fsPos, int idx) in input[0].Enumerate())
        {
            bool isFreeBlock = idx % 2 == 1;
            ulong size = ulong.Parse($"{fsPos}");
            ulong? id = isFreeBlock ? null : fileId;
            while (size > 0)
            {
                fs.Add( (isFreeBlock, id));
                size--;
            }

            if (!isFreeBlock)
            {
                fileId++;
            }
        }

        int i = 0, j = fs.Count - 1;
        while (true)
        {
            while (!fs[i].Item1)
            {
                i++;
            }

            while (fs[j].Item1)
            {
                j--;
            }

            if (i >= j)
            {
                break;
            }

            (fs[i], fs[j]) = (fs[j], fs[i]);
        }

        ulong checksum = 0;
        foreach((ValueTuple<bool, ulong?> block, int idx) in fs.Enumerate())
        {
            checksum += (ulong)idx * (block.Item2 ?? 0);
        }
        return checksum;
    }

    // this technically runs faster, but the linked list solution looks better and works as well 
    private static ulong Part2Optimized(string input)
    {
        PriorityQueue<int, int>[] minFreeSpaces = new PriorityQueue<int, int>[10];
        for (int heap = 1; heap < minFreeSpaces.Length; heap++)
        {
            minFreeSpaces[heap] = new PriorityQueue<int, int>();
        }
        List<ValueTuple<bool, ulong?>> fs = [];
        
        ulong fileId = 0;
        foreach ((char fsPos, int idx) in input.Enumerate())
        {
            bool isFreeBlock = idx % 2 == 1;
            int size = int.Parse($"{fsPos}");
            ulong? id = isFreeBlock ? null : fileId++;
            if (isFreeBlock && size > 0)
            {
                minFreeSpaces[size].Enqueue(fs.Count, fs.Count);
            }
            while (size > 0)
            {
                fs.Add( (isFreeBlock, id));
                size--;
            }
        }
        
        for(int i = fs.Count - 1; i >= 0;)
        {
            if (fs[i].Item1)
            {
                i--;
                continue;
            }

            ulong file = fs[i].Item2!.Value;
            int size = 0;
            while (i >= 0 && fs[i].Item2 == file)
            {
                size++;
                i--;
            }

            int freeIdx = int.MaxValue, bestHeap = int.MaxValue;
            for (int heap = size; heap < minFreeSpaces.Length; heap++)
            {
                if (!minFreeSpaces[heap].TryPeek(out int idx, out int _) || idx >= freeIdx)
                {
                    continue;
                }
                bestHeap = heap;
                freeIdx = idx;
            }

            if (bestHeap == int.MaxValue || freeIdx > i)
            {
                continue;
            }
            minFreeSpaces[bestHeap].Dequeue();
            if (bestHeap > size)
            {
                minFreeSpaces[bestHeap-size].Enqueue(freeIdx+size, freeIdx+size);
            }
            while (size > 0)
            {
                (fs[freeIdx + size-1], fs[i+size]) = (fs[i+size], fs[freeIdx + size-1]);
                size--;
            }
        }
        ulong checksum = 0;
        foreach((ValueTuple<bool, ulong?> block, int idx) in fs.Enumerate())
        {
            checksum += (ulong)idx * (block.Item2 ?? 0);
        }
        return checksum;
    }
    
    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        
        LinkedList<FsBlock> fs = [];
        
        ulong fileId = 0;
        
        foreach ((char fsPos, int idx) in input[0].Enumerate())
        {
            bool isFreeBlock = idx % 2 == 1;
            ulong size = ulong.Parse($"{fsPos}");
            ulong? id = isFreeBlock ? null : fileId++;
            fs.AddLast(new FsBlock { IsEmpty = isFreeBlock, Id = id, Size = size });
        }
        
        LinkedListNode<FsBlock>? currBlock = fs.Last;
        while (true)
        {
            LinkedListNode<FsBlock>? freeBlock = fs.First;
            // advance if current block is empty
            while (currBlock != fs.First && currBlock is { Value.IsEmpty: true })
            {
                currBlock = currBlock.Previous;
            }
            // we are finished
            if (currBlock == null || currBlock == fs.First)
            {
                break;
            }

            while ((freeBlock is { Value.IsEmpty: false } || freeBlock?.Value.Size < currBlock.Value.Size) && freeBlock != currBlock)
            {
                freeBlock = freeBlock.Next;
            }

            if (freeBlock == null || freeBlock == currBlock)
            {
                // no fitting free blocks left;
                currBlock = currBlock.Previous;
                continue;
            }

            ulong remainder = freeBlock.Value.Size - currBlock.Value.Size;
            freeBlock.Value.Size -= remainder;
            if (remainder != 0)
            {
                if (freeBlock.Next?.Value?.IsEmpty ?? false)
                {
                    freeBlock.Next.Value.Size += remainder;
                }
                else
                {
                    fs.AddAfter(freeBlock, new FsBlock
                    {
                        Size = remainder,
                        IsEmpty = true,
                    });
                }
            }

            freeBlock.Value.IsEmpty = false;
            freeBlock.Value.Id = currBlock.Value.Id;
            currBlock.Value.IsEmpty = true;
            currBlock.Value.Id = null;

            // this needs to be done for the accumulation
            if (currBlock.Previous?.Value.IsEmpty ?? false)
            {
                currBlock.Value.Size += currBlock.Previous.Value.Size;
                fs.Remove(currBlock.Previous);
            }
            if (currBlock.Next?.Value.IsEmpty ?? false)
            {
                currBlock.Value.Size += currBlock.Next.Value.Size;
                fs.Remove(currBlock.Next);
            }
            currBlock = currBlock.Previous;
        }
        ulong checksum = 0;
        ulong blockIdx = 0;
        foreach(FsBlock block in fs)
        {
            checksum += (block.Size) * (2 * blockIdx + block.Size - 1) / 2 * (block.Id ?? 0);
            blockIdx += block.Size;
        }
        return checksum;
    }
}