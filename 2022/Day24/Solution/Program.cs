using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        private static Dictionary<int, HashSet<Point>> _mapCache = new();

        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        // wrong: 1528 high
        public static int Part1(Input input)
        {



            var startLocation = new Point(1, 0);
            //var endLocation = new Point(6, 5);
            var endLocation = new Point(150, 21);

            var foundPaths = new List<Point[]>();

            FindPath(input, startLocation, endLocation, 0, new(), foundPaths);

            var path = foundPaths.OrderBy(p => p.Length).FirstOrDefault();
            //for (var i = 0; i < 100; i++)
            //{
            //    var blizzards = GetObstructedCells(input, i);
            //    Print(blizzards, input.World);
            //    Console.ReadKey();
            //}
            return 0;
        }

        private static void FindPath(Input input, Point currentLocation, Point target, int minute, Stack<Point> currentPath, List<Point[]> foundPaths)
        {
            //Console.Write(currentPath);
            if (currentPath.Count > 2500 || (foundPaths.Count > 0 && currentPath.Count > foundPaths.Min(p => p.Length)))
                return;

            if (currentLocation == target)
            {
                currentPath.Push(currentLocation);
                foundPaths.Add(currentPath.Reverse().ToArray());
                currentPath.Pop();
                return;
            }

            var moves = GetPossibleMoves(currentLocation, GetObstructedCells(input, minute + 1)).OrderBy(move => target.Subtract(move).ManhattanDistance);

            currentPath.Push(currentLocation);
            foreach (var move in moves)
            {
                FindPath(input, move, target, minute + 1, currentPath, foundPaths);
            }
            currentPath.Pop();
        }

        public static int Part2(Input input)
        {
            return 0;
        }

        private static IEnumerable<Point> GetPossibleMoves(Point location, HashSet<Point> blockedPoints)
        {
            return Shapes.Neighbours(location, false)
                .Concat(new[] { location })
                .Where(p => !blockedPoints.Contains(p))
                .Where(p => p.X >= 0 && p.Y >= 0 && p.X <= 150 && p.Y <= 21);
        }

        private static IEnumerable<Point> GetBlizzardLocations(IEnumerable<Blizzard> blizzards, int minute)
        {
            return blizzards.Select(b => b.GetLocationAtMinute(minute));
        }

        private static HashSet<Point> GetObstructedCells(Input input, int minute)
        {
            if (!_mapCache.TryGetValue(minute, out var locations))
            {
                locations = new(input.World.AllCells().Where(c => c.Value == '#').Select(c => c.Location)
                    .Concat(GetBlizzardLocations(input.Blizzards, minute))
                );
                _mapCache.Add(minute, locations);
            }

            return locations;
        }

        public static void Print(HashSet<Point> blizzards, Grid<char> grid)
        {
            Console.Clear();

            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    if (blizzards.Contains(new(x, y)))
                        Console.Write("#");
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
        }
    }
}
