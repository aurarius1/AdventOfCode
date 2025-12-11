namespace _2025.Utils;

public static class MathExtensions
{
    public static int Modulo(int a, int n) => (a % n + n) % n;
    
    public static long Modulo (long a, long n) => (a % n + n) % n;
    
    public static ulong Modulo (ulong a, ulong n) => (a % n + n) % n;

    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long t = b;
            b = a % b;
            a = t;
        }
        return Math.Abs(a);
    }
    
}