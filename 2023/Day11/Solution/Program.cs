using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;


public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 9742154 || part2 != 411142919886L)
        {
            throw new Exception();
        }
    }

    public static long Part1(Grid<char> input)
    {
        return Solve(input, 2);
    }

    public static long Part2(Grid<char> input)
    {
        return Solve(input, 1_000_000);
    }

    private static long Solve(Grid<char> input, int expansion)
    {
        var galaxies = input.AllCells().Where(cell => cell.Value == '#').Select(cell => cell.Location).ToList();

        galaxies = ExpandSpace(expansion, galaxies);

        return CreatePairs(galaxies)
            .Sum(pair => (long)pair.Item1.ManhattanDistanceTo(pair.Item2));
    }

    private static List<Point> ExpandSpace(int expansion, List<Point> galaxies)
    {
        var maxX = galaxies.Max(g => g.X);
        var maxY = galaxies.Max(g => g.Y);
        var galaxiesX = Enumerable.Range(0, maxX + 1).Select(index => galaxies.Count(g => g.X == index)).ToArray();
        var galaxiesY = Enumerable.Range(0, maxY + 1).Select(index => galaxies.Count(g => g.Y == index)).ToArray();

        return galaxies.Select(galaxy =>
        {
            var countX = galaxiesX.Where((count, index) => index < galaxy.X && count == 0).Count();
            var countY = galaxiesY.Where((count, index) => index < galaxy.Y && count == 0).Count();

            return new Point(
                galaxy.X + (countX * (expansion - 1)),
                galaxy.Y + (countY * (expansion - 1))
            );
        }).ToList();
    }

    private static HashSet<Tuple<Point, Point>> CreatePairs(List<Point> galaxies)
    {
        var pairs = new HashSet<Tuple<Point, Point>>();

        for (var i = 0; i < galaxies.Count; i++)
        {
            for (var j = 1; j < galaxies.Count; j++)
            {
                if (i == j)
                    continue;

                if (!pairs.Contains(new Tuple<Point, Point>(galaxies[j], galaxies[i])))
                {
                    pairs.Add(new Tuple<Point, Point>(galaxies[i], galaxies[j]));
                }
            }
        }

        return pairs;
    }
}
