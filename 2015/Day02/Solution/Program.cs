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
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(List<int[]> input)
        {
            return input.Select(package => new[] {
                    package[0] * package[1],
                    package[1] * package[2],
                    package[0] * package[2],
                }).Sum(sides => sides.Sum(side => side * 2) + sides.Min());
        }

        public static int Part2(List<int[]> input)
        {
            return input.Sum(package =>
                package.OrderBy(v => v).Take(2).Sum(side => side + side) + package.Aggregate((x, y) => x * y));
        }
    }
}
