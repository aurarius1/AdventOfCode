namespace _2024.Utils;

public static class MathExtensions
{
    public static int Modulo(int a, int n) => (a % n + n) % n;
    
    public static long Modulo (long a, long n) => (a % n + n) % n;
}