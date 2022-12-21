namespace Shared
{
    public static class StringExtensions
    {
        public static string StripPrefix(this string str, string prefix)
        {
            return str.Substring(prefix.Length);
        }

        public static string[] ToSections(this string str) => str.Split(Environment.NewLine + Environment.NewLine);
        public static string[] ToLines(this string str) => str.Split(Environment.NewLine);
        public static long ParseLong(this string str) => long.Parse(str.Trim());
        public static int ParseInt(this string str) => int.Parse(str.Trim());

        public static long[] ParseLongArray(this string str)
        {
            return str.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        public static int[] ParseIntArray(this string str)
        {
            return str.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public static bool IsNumber(this string str) => str.ToArray().All(char.IsDigit);
    }
}
