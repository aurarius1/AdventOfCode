namespace _2024.Utils;

public class MathExtensions
{
    public static int Mod(int a, int n) => (a % n + n) % n;
}