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

            if (part1 != 2679 || part2 != 77607)
            {
                throw new Exception();
            }
        }

        public static int Part1(Game[] input)
        {
            var availableCubes = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 },
            };

            return input.Where(game =>
                game.Sets
                    .SelectMany(set => set)
                    .All(cubes => availableCubes[cubes.Color] >= cubes.Quantity)
                )
                .Sum(game => game.Id);
        }

        public static int Part2(Game[] input)
        {
            return input.Sum(game =>
                game.Sets
                    .SelectMany(set => set)
                    .GroupBy(set => set.Color, set => set.Quantity)
                    .Select(group => group.Max())
                    .Aggregate((x, y) => x * y)
            );
        }
    }
}
