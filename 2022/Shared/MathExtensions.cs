namespace Shared
{
    public static class MathFunctions
    {
        public static long Factorial(int input)
        {
            if (input == 0)
            {
                return 1;
            }

            return input * Factorial(input - 1);
        }
    }
}
