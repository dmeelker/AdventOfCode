using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public static class Directions
    {
        public static readonly Point NorthWest = new Point(0, -1);
        public static readonly Point NorthEast = new Point(1, -1);

        public static readonly Point West = new Point(-1, 0);
        public static readonly Point East = new Point(1, 0);

        public static readonly Point SouthWest = new Point(-1, 1);
        public static readonly Point SouthEast = new Point(0, 1);

        public static readonly Point[] All = new[] { NorthWest, NorthEast, West, East, SouthWest, SouthEast };
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(part1);

            Console.WriteLine($"Part 1: {part1.Count()} Part 2: {part2.Count}");
        }

        public static HashSet<Point> Part1(Point[][] input)
        {
            return input.Select(path => path.Aggregate((p1, p2) => p1.Add(p2)))
                .GroupBy(point => point)
                .Where(g => g.Count() % 2 == 1)
                .Select(g => g.Key)
                .ToHashSet();
        }

        public static HashSet<Point> Part2(HashSet<Point> blackTiles)
        {
            foreach (var step in Enumerable.Range(0, 100))
            {
                blackTiles = Enumerable.Concat(
                    blackTiles.Where(tile => InRange(CountAdjacentBlackTiles(tile, blackTiles), 1, 2)), // Black tiles with 1 or 2 black neighbours
                    GetWhiteTiles(blackTiles).Where(tile => CountAdjacentBlackTiles(tile, blackTiles) == 2) // White tiles with 2 black neighbours
                ).ToHashSet();
            }

            return blackTiles;
        }

        public static bool InRange(int value, int from, int to)
        {
            return value >= from && value <= to;
        }

        public static int CountAdjacentBlackTiles(Point point, HashSet<Point> blackTiles)
        {
            return GetAdjacentCells(point).Count(blackTiles.Contains);
        }

        public static IEnumerable<Point> GetWhiteTiles(HashSet<Point> blackTiles)
        {
            return blackTiles.SelectMany(GetAdjacentCells).Distinct().Where(p => !blackTiles.Contains(p));
        }

        public static IEnumerable<Point> GetAdjacentCells(Point p)
        {
            return Directions.All.Select(d => p.Add(d));
        }
    }
}
