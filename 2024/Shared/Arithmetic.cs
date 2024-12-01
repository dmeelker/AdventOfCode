namespace Shared;

public static class Arithmetic
{
    public static long LCM(long a, long b)
    {
        return a * b / GCD(a, b);
    }

    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}
