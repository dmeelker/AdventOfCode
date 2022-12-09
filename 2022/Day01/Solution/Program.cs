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
            var input = ParseInput(File.ReadAllText("input.txt")).ToList();
            var part1 = input.Max();
            var part2 = input.OrderByDescending(value => value).Take(3).Sum();

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static IEnumerable<long> ParseInput(string input)
        {
            return input.Split(Environment.NewLine + Environment.NewLine)
                .Select(section => section.Split(Environment.NewLine).Select(long.Parse).Sum());
        }
    }
}
