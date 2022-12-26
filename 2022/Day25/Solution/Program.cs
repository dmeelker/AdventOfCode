using System;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);

            Console.WriteLine($"Part 1: {part1}");
        }

        public static string Part1(string[] input)
        {
            return ConvertToSnafu(input.Sum(ParseSnafu));
        }

        public static long ParseSnafu(string input)
        {
            var sum = 0L;

            for (var i = 0; i < input.Length; i++)
            {
                var chr = input[input.Length - i - 1];
                long value = GetSnafuValue(chr);
                if (i > 0)
                {
                    value *= (long)Math.Pow(5, i);
                }
                sum += value;

            }

            return sum;
        }

        private static int GetSnafuValue(char chr)
        {
            return chr switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new ArgumentException()
            };
        }

        public static string ConvertToSnafu(long value)
        {
            if (value == 0)
                return string.Empty;

            var digit = new[] { '0', '1', '2', '=', '-' }[(int)(value % 5.0)];
            return ConvertToSnafu((value + 2) / 5) + digit;
        }
    }
}
