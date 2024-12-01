namespace Shared
{
    public static class StringExtensions
    {

        public static string StripPrefix(this string str, string prefix)
        {
            return str.Substring(prefix.Length);
        }

        public static string StripPostfix(this string str, string postfix)
        {
            return str.Substring(0, str.Length - postfix.Length);
        }

        public static string[] ToSections(this string str) => str.Split(Environment.NewLine + Environment.NewLine);
        public static string[] ToLines(this string str) => str.Split(Environment.NewLine);
        public static long ParseLong(this string str) => long.Parse(str.Trim());
        public static int ParseInt(this string str) => int.Parse(str.Trim());

        public static long[] ParseLongArray(this string str)
        {
            return str.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        public static int[] ParseIntArray(this string str, params string[] separators)
        {
            return str.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public static T ScanLeft<T>(this string input, Func<string, T?> matchFunction)
        {
            while (input.Length > 0)
            {
                var result = matchFunction(input);
                if (result != null) return result;
                input = input.Substring(1);
            }

            throw new Exception();
        }

        public static T ScanRight<T>(this string input, Func<string, T?> matchFunction)
        {
            while (input.Length > 0)
            {
                var result = matchFunction(input);
                if (result != null) return result;
                input = input.Substring(0, input.Length - 1);
            }

            throw new Exception();
        }

        public static bool TryParseDigitAt(this string input, Index index, out int result)
        {
            if (char.IsDigit(input[index]))
            {
                result = int.Parse(char.ToString(input[index]));
                return true;
            }
            result = default;
            return false;
        }
    }
}
