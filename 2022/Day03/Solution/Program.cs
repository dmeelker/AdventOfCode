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
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(List<IEnumerable<char>> input)
        {
            return input.Sum(line =>
                line.Split().Aggregate((x, y) => x.Intersect(y))
                    .Sum(GetPriority)
                );
        }

        public static int Part2(List<IEnumerable<char>> input)
        {
            return input.Chunk(3)
                .Sum(chunk =>
                    chunk.Aggregate((a, b) => a.Intersect(b))
                        .Sum(GetPriority)
                );
        }

        private static int GetPriority(char itemType)
        {
            if (itemType >= 'a')
            {
                return (itemType - 'a') + 1;
            }
            else
            {
                return (itemType - 'A') + 27;
            }
        }
    }
}
