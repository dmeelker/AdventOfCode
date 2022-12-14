using Shared;
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
            var input = File.ReadAllText("input.txt");
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(string input)
        {
            return Solve(input, 1);
        }

        public static int Part2(string input)
        {
            return Solve(input, 2);
        }

        private static int Solve(string input, int santaCount)
        {
            var visitSet = new HashSet<Point>();
            var locations = Enumerable.Repeat(new Point(0, 0), santaCount).ToArray();
            var stepIndex = 0;

            foreach (var step in input)
            {
                var locationIndex = stepIndex % locations.Length;
                visitSet.Add(locations[locationIndex]);

                var v = step switch
                {
                    '<' => new Point(-1, 0),
                    '>' => new Point(1, 0),
                    '^' => new Point(0, -1),
                    'v' => new Point(0, 1),
                    _ => throw new Exception()
                };
                locations[locationIndex] = locations[locationIndex].Add(v);
                stepIndex++;
            }

            return visitSet.Count;
        }
    }
}
