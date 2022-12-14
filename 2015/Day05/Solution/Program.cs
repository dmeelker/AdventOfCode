using Shared;
using System;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(string[] input)
        {
            return input.Count(IsNice);
        }

        private static bool IsNice(string str)
        {
            if (str.ToCharArray().Count("aeiou".Contains) < 3)
            {
                return false;
            }

            if (!str.ToCharArray().SlidingWindow(2).Any(pair => pair[0] == pair[1]))
            {
                return false;
            }

            var badStrings = new[] { "ab", "cd", "pq", "xy" };
            if (badStrings.Any(str.Contains))
            {
                return false;
            }

            return true;
        }


        public static int Part2(string[] input)
        {
            return input.Count(IsNice2);
        }

        private static bool IsNice2(string arg)
        {
            if (!ContainsNonOverlappingPair(arg))
            {
                return false;
            }

            if (!arg.ToCharArray().SlidingWindow(3).Any(window => window[0] == window[2]))
            {
                return false;
            }

            return true;
        }

        private static bool ContainsNonOverlappingPair(string arg)
        {
            for (var i = 0; i < arg.Length - 2; i++)
            {
                var pair = arg.Substring(i, 2);

                if (arg.Substring(i + 2).Contains(pair))
                    return true;
            }

            return false;
        }
    }
}
