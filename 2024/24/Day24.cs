
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using _2024.Utils;

namespace _2024._24;

public enum Operator
{
    Xor, 
    And,
    Or
}

public struct Gate
{
    public string InputWire1 { get; init; }
    public string InputWire2 { get; init; }
    public string OutputWire { get; set; }
    public Operator Operator { get; init; }


    public bool IsXAndYInput()
    {
        return InputWire1[0] == 'x' && InputWire2[0] == 'y' || InputWire1[0] == 'y' && InputWire2[0] == 'x';
    }

    public bool TryPerformOperation(Dictionary<string, bool> wires)
    {
        if (!wires.TryGetValue(InputWire1, out bool wire1) || !wires.TryGetValue(InputWire2, out bool wire2))
        {
            return false;
        }
        wires[OutputWire] = Operator switch
        {
            Operator.And => wire1 && wire2,
            Operator.Or => wire1 || wire2,
            Operator.Xor => wire1 ^ wire2,
            _ => throw new Exception("Unknown operator")
        };
        
        return true;
    }

    public override string ToString()
    {
        return $"({InputWire1}, {InputWire2}, {OutputWire}, {Operator})"; 
    }
}

public class Day24 : Base
{
    private readonly Dictionary<string, bool> _wires = new();
    private readonly List<Gate> _gates = [];
    
    public Day24(bool example) : base(example)
    {
        Day = "24";

        string[] input = ReadInput();
        foreach ((string line, int idx) in input.Enumerate())
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts;
            if (line.Contains(':'))
            {
                 parts = line.Split(": ");
                _wires[parts[0]] = parts[1] == "1";
                continue;
            }

            parts = line.Replace(" -> ", " ").Split(' ');
            _gates.Add(new Gate
            {
                InputWire1 = parts[0],
                InputWire2 = parts[2],
                OutputWire = parts[^1],
                Operator = parts[1].ToLower() switch
                {
                    "and" => Operator.And,
                    "or" => Operator.Or,
                    "xor" => Operator.Xor,
                    _ => throw new Exception("operator invalid")
                }
            });
        }
    }

    private long GetResult(IEnumerable<string> wires) => Convert.ToInt64(string.Join("", wires.Select(x => _wires[x] ? "1" : "0")), 2);
    
    private string FindPair(string wire)
    {
        // first check the "current" level if there are any gates that output to a zXX wire
        List<string> result = _gates
            .Where(x => ((x.InputWire1 == wire || x.InputWire2 == wire)&& x.OutputWire[0] == 'z'))
            .Select(x => $"z{(int.Parse(x.OutputWire[1..]) - 1):D2}")
            .ToList();
        
        // we found one, get the first match
        // this could be omitted, we would still return the first match afterwards, but we would need an AddRange
        if (result.Count >= 1)
        {
            return result.First();
        }
        
        // if there are none, find all one level deeper
        result = _gates
            .Where(x => x.InputWire1 == wire || x.InputWire2 == wire)
            .Select(x => FindPair(x.OutputWire))
            .ToList();
        return result.FirstOrDefault() ?? string.Empty;
    }

    public override object PartOne()
    {
        Queue<Gate> queue = new (_gates);
        while (queue.TryDequeue(out Gate gate))
        {
            if (!gate.TryPerformOperation(_wires))
            {
                queue.Enqueue(gate);
            }
        }
        
        return GetResult(_wires.Keys.Where(x => x[0] == 'z').OrderByDescending(x => x));
    }

    public override object PartTwo()
    {
        List<int> outputToZButNoXor = [], intermediateInputButXor = [];
        foreach ((Gate gate, int idx) in _gates.Enumerate())
        {
            if (gate.OutputWire[0] == 'z')
            {
                if (gate.Operator != Operator.Xor && gate.OutputWire != "z45")
                {
                    outputToZButNoXor.Add(idx); 
                }
                
                continue;
            }
            
            if (gate.IsXAndYInput())
            {
                continue;
            }
            
            if (gate.Operator == Operator.Xor)
            {
                intermediateInputButXor.Add(idx);
            }
        }
        
        foreach (int gate in intermediateInputButXor)
        {
            int otherGateIdx = outputToZButNoXor.FirstOrDefault(x => FindPair(_gates[gate].OutputWire) == _gates[x].OutputWire);
            Gate gate1 = _gates[otherGateIdx];
            Gate gate2 = _gates[gate];
            (gate1.OutputWire, gate2.OutputWire) = (gate2.OutputWire, gate1.OutputWire);
            _gates[otherGateIdx] = gate1;
            _gates[gate] = gate2;
        }
        
        long x = GetResult(_wires.Keys.Where(x => x[0] == 'x').OrderByDescending(x => x));
        long y = GetResult(_wires.Keys.Where(y => y[0] == 'y').OrderByDescending(y => y));

        long expectedAnswer = x + y;
        long answer = (long)PartOne();
        string binary = Convert.ToString(answer ^ expectedAnswer, 2); 
        string faultyAdder = $"{binary.Count(bit => bit.Equals('0')):D2}";
        
        List<string> faultyWires = _gates.Where(gate => gate.InputWire1[1..] == faultyAdder  && gate.InputWire2[1..] == faultyAdder).Select(gate => gate.OutputWire).ToList();
        faultyWires.AddRange(intermediateInputButXor.Select(gate => _gates[gate].OutputWire));
        faultyWires.AddRange(outputToZButNoXor.Select(gate => _gates[gate].OutputWire));
        
        return string.Join(",", faultyWires.OrderBy(faultyWire => faultyWire));
    }

    public override void Reset()
    {
        // reset the intermediate wires
        _wires.Keys.Where(x => x[0] != 'y' && x[0] != 'x').ToList().ForEach(x => _wires.Remove(x));
    }
}

