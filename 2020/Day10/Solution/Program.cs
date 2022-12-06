using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = PreprocessInput(ParseInput(File.ReadAllText("input.txt")));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static long[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => long.Parse(line.Trim()))
                .ToArray();
        }

        public static long[] PreprocessInput(long[] input)
        {
            input = input.OrderBy(value => value).ToArray();
            return input.Union(new long[] { 0, input.Last() + 3 })
                .OrderBy(value => value)
                .ToArray();
        }

        public static int Part1(long[] input)
        {
            var counts = Enumerable.Range(0, input.Length - 1)
                .Select(i => input[i + 1] - input[i])
                .GroupBy(diff => diff)
                .ToDictionary(group => group.Key, group => group.Count());

            return counts[1] * counts[3];
        }

        public static long Part2(long[] input)
        {
            var moveCache = new Dictionary<int, long>();
            CachePossibleMoves(0, moveCache);
            return moveCache[0];

            bool CanReach(int from, int target)
            {
                if (target >= input.Length)
                    return false;

                return input[target] - input[from] <= 3;
            }

            long CachePossibleMoves(int from, Dictionary<int, long> moveCache)
            {
                if (moveCache.ContainsKey(from))
                    return moveCache[from];

                if (from == input.Length - 1)
                    return 1;

                return moveCache[from] = Enumerable.Range(from + 1, 3)
                    .Where(step => CanReach(from, step))
                    .Select(step => CachePossibleMoves(step, moveCache))
                    .DefaultIfEmpty(0)
                    .Sum();
            }
        }
    }
}
