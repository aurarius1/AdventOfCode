using System.Diagnostics;
using System;

namespace _2025._10
{
    // check with this: https://github.com/romamik/aoc2025/blob/master/day10/day10p2.py
    public class Rational
    {
        
        private long _numerator, _denominator;
        public Rational(long numerator, long denominator)
        {
            if (denominator == 0)
                denominator = 1; 
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
            
            long gcd = MathExtensions.GCD(Math.Abs(numerator), Math.Abs(denominator));
            _numerator = numerator / gcd;
            _denominator = denominator / gcd;
        }
        
        public long Numerator => _numerator;
        public long Denominator => _denominator;
        
        // Addition
        public static Rational operator +(Rational a, Rational b)
        {
            long num = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
            long den = a.Denominator * b.Denominator;
            return new Rational(num, den);
        }

        // Subtraction
        public static Rational operator -(Rational a, Rational b)
        {
            long num = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
            long den = a.Denominator * b.Denominator;
            return new Rational(num, den);
        }

        // Multiplication
        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }
        
        public static Rational operator *(long a, Rational b)
        {
            return new Rational(a * b.Numerator,  b.Denominator);
        }

        // Division
        public static Rational operator /(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        public static bool operator <(Rational a, Rational b)
        {
            return a.Numerator * b.Denominator < b.Numerator * a.Denominator;
        }

        public static bool operator >(Rational a, Rational b)
        {
            return a.Numerator * b.Denominator > b.Numerator * a.Denominator;
        }

        public static bool operator <=(Rational a, Rational b) => a < b || a == b;
        public static bool operator >=(Rational a, Rational b) => a > b || a == b;

// Also override Equals and GetHashCode for completeness
        public static bool operator ==(Rational? a, Rational? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null) return false;
            return a.Numerator * b.Denominator == b.Numerator * a.Denominator;
        }
        

        public static bool operator !=(Rational a, Rational b) => !(a == b);
        public override string ToString() => $"{Numerator}/{Denominator}";
        public long ToLong() => Numerator == 0 ? 0L : (long)Math.Round(Numerator / (double)Denominator, MidpointRounding.AwayFromZero);
    }
    public class LinearExpression
    {
        
        public Dictionary<int, Rational> Coefficients = [];
        public Rational Constant = new(0, 1);
        
        public Rational Evaluate(Dictionary<int, long> freeValues)
        {
            Rational sum = Constant; // start with the constant
            foreach (var kvp in Coefficients)
            {
                sum += new Rational(freeValues[kvp.Key], 1) * kvp.Value;
            }
            return sum;
        }

        public void AddCoefficient(int varIndex, Rational coef)
        {
            if (!Coefficients.TryAdd(varIndex, coef))
            {
                Coefficients[varIndex] += coef;
            }
        }

        public override string ToString()
        {
            if (Coefficients.Count == 0)
            {
                return Constant.ToString();
            }
            string result = Constant.ToString() + " - (" + $"({Coefficients[Coefficients.Keys.First()]} * x_{Coefficients.Keys.First()})";
            foreach (var coefficient in Coefficients.Keys.Skip(1))
            {
                
                result += $" + ({Coefficients[coefficient]} * x_{coefficient})";
            }
            result += ")";
            return result;
        }
    }
    public sealed class Day10 : Base
    {
        private double TOLERANCE = 1e-14;
        public Day10(bool example) : base(example)
        {
            Day = "10";
        }

        private int FindMinButtonPresses(int lights, List<int[]> buttons)
        {
            HashSet<int> seen = [];
            Queue<(int, int)> states = new();
            states.Enqueue((0, 0));
            while (states.Count > 0)
            {
                (int steps, int curr) = states.Dequeue();
                if (!seen.Add(curr))
                {
                    continue;
                }

                if (curr == lights)
                {
                    return steps;
                }

                foreach (var currButtons in buttons)
                {
                    states.Enqueue((steps + 1,
                        currButtons.Aggregate(curr, (current, button) => current ^ (1 << button))));
                }
            }

            return 0;
        }

        public override object PartOne()
        {
            int fewestPresses = 0;
           
            
            foreach (var machine in ReadInput().Select(x => x.Split(' ')))
            {
     
                int light = Convert.ToInt32(new string(machine[0][1..^1]
                    .Replace('.', '0')
                    .Replace('#', '1')
                    .Reverse()
                    .ToArray()
                ), 2);
                List<int[]> sequences = machine
                    .Skip(1)
                    .SkipLast(1)
                    .Select(part => part[1..^1].Split(',').Select(int.Parse).ToArray())
                    .ToList();
                fewestPresses += FindMinButtonPresses(light, sequences);
            }

            return fewestPresses;
        }
        
        private bool Pivot(long[][] matrix, (int Row, int Col) position)
        {
            int pivotRow = position.Row;
            long maxAbs = Math.Abs(matrix[pivotRow][position.Col]);
            for (int i = position.Row + 1; i < matrix.Length; i++)
            {
                long absVal = Math.Abs(matrix[i][position.Col]);
                if (absVal > maxAbs)
                {
                    maxAbs = absVal;
                    pivotRow = i;
                }
            }

            if (maxAbs == 0)
            {
                return false;
            }

            if (pivotRow != position.Row)
            {
                
                (matrix[position.Row], matrix[pivotRow]) = (matrix[pivotRow], matrix[position.Row]);
            }

            if (matrix[position.Row][position.Col] >= 0)
            {
                return true;
            }
            
            for (int i = position.Col; i < matrix[position.Row].Length; i++)
            {
                matrix[position.Row][i] *= -1;
            }
            


            return true;
        }
        
        private LinearExpression?[] _solutions = [];
        
        
        
        private Rational? SearchSolutions(int level, (int variable,  int lower, int upper)[] bounds, Dictionary<int, long> freeValues)
        {
            
            if (level == bounds.Length)
            {
                // we are done;
                List<Rational> solvedExpressions = _solutions
                    .Select((x, i) => x?.Evaluate(freeValues) ?? new(freeValues[i], 1))
                    .ToList();
                return solvedExpressions.Any(x => x.Numerator < 0) ? null : solvedExpressions.Aggregate(new Rational(0, 1), (acc, x) => acc + x);
            }

            Rational? sum = null;
            for (long i = bounds[level].lower; i <= bounds[level].upper; i++)
            {
                freeValues[bounds[level].variable] = i;
                var tmp = SearchSolutions(level + 1, bounds, freeValues);
                if (tmp == null)
                {
                    continue;
                }
                if (sum == null || tmp < sum)
                {
                    sum = tmp;
                }
            }

            return sum;
        }
        
        public override object PartTwo()
        {
            // TODO for my solution it works, for another input it is off by 3 ... don't know why
            long fewestPresses = 0;
            foreach (var machine in ReadInput().Select(x => x.Split(' ')))
            {
                int[] joltages = machine[^1][1..^1].Split(',').Select(int.Parse).ToArray();
                List<int[]> sequences = machine
                    .Skip(1)
                    .SkipLast(1)
                    .Select(part => part[1..^1].Split(',').Select(int.Parse).ToArray())
                    .ToList();

                long[][] equations = new long[joltages.Length][];

                foreach ((int joltage, int index) in joltages.Select((x, i) => (x, i)))
                {
                    IEnumerable<long> data = sequences.Select(x => x.Contains(index) ? 1L : 0L);
                    if (joltages.Length - sequences.Count > 0)
                    {
                        data = data.Concat(Enumerable.Repeat(0L, joltages.Length - sequences.Count));
                    }
                    equations[index] = data.Append(joltage).ToArray();
                }
                //foreach ((var equation, int index) in equations.Select((x, i) => (x, i)))
                for(int index = 0; index < equations.Length; index++)
                {
                    for (int col = index; col < equations[index].Length; col++)
                    {
                        if (!Pivot(equations, (index, col)))
                        {
                            continue;
                        }
                        long pivotCol = equations[index][col];
                        // we can eliminate all rows below
                        foreach (var equation in equations.Skip(index + 1))
                        {
                            long rowPivotCol = equation[col];
                            if (rowPivotCol == 0)
                            {
                                continue;
                            }
  
                            long rowGcd = equation[col];
                            for (int col3 = col; col3 < equation.Length; col3++)
                            {
                                equation[col3] = equation[col3] * pivotCol - equations[index][col3] * rowPivotCol;
                                if (col3 > col)
                                {
                                    rowGcd = MathExtensions.GCD(rowGcd, equation[col3]);
                                }
                            }

                            if (rowGcd == 0)
                            {
                                continue;
                            }
                            for (int c = col+1; c < equation.Length; c++)
                            {
                                equation[c] /= rowGcd;
                                
                            }
                        }
                        break;
                    }
                }
                
                /*foreach (var equation in equations)
                {
                    Console.Write(string.Join(" + ", equation.SkipLast(1).Select(x => x.ToString().PadLeft(2, ' '))));
                    Console.Write(" = ");
                    Console.WriteLine(equation.Last());
                }
                Console.WriteLine();*/
                
                _solutions = new LinearExpression?[sequences.Count];
                for (int row = equations.Length - 1; row >= 0; row--)
                {
                    LinearExpression expr = new();
                    int? firstVariable = null;
                    for (int col = 0; col <= equations[row].Length - 2; col++)
                    {
                        if (equations[row][col] == 0)
                        {
                            continue;
                        }

                        if (firstVariable == null)
                        {
                            firstVariable = col;
                            expr.Constant = new Rational(equations[row][^1], equations[row][col]);
                            continue;
                        }
                        
                        if (_solutions[col] == null)
                        {
                            expr.AddCoefficient(col, new Rational(-equations[row][col], equations[row][firstVariable.Value]));
                            continue;
                        }

                        foreach (var solved in _solutions[col]!.Coefficients)
                        {
                            expr.AddCoefficient(solved.Key, new Rational(-equations[row][col], equations[row][firstVariable.Value]) * solved.Value);
                        }
                        expr.Constant -= new Rational(equations[row][col], equations[row][firstVariable.Value]) * _solutions[col]!.Constant;

                    }

                    if (firstVariable != null)
                    {
                        _solutions[firstVariable.Value] = expr;
                    }
                }

                /*foreach (var solution in _solutions.Select((x, i) => (x, i)))
                {
                    if (solution.x == null)
                    {
                        continue;
                    }
                    Console.Write($"{solution.i}: ");
                    Console.WriteLine(solution.x.ToString());
                }*/
                
                (int variable, int lower, int upper)[] bounds = _solutions
                    .Select((x, i) => (i, 0, joltages.Where((y, j) => sequences[i].Contains(j)).Min()))
                    .Where(x => _solutions[x.i] == null)
                    .ToArray();
                Rational? presses = SearchSolutions(0, bounds, []);
                fewestPresses += presses?.ToLong() ?? 0;
            }
            return fewestPresses;
        }
    }
}
