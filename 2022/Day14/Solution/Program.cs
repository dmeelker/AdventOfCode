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

            if (part1 != 674 || part2 != 24958)
            {
                throw new Exception();
            }
        }

        public static int Part1(List<Path> paths)
        {
            return Solve(paths, 1);
        }

        public static int Part2(List<Path> paths)
        {
            return Solve(paths, 2);
        }

        private static int Solve(List<Path> paths, int part)
        {
            var field = PrepareField(paths, part);
            var maxY = field.Keys.Max(point => point.Y);

            while (true)
            {
                var sandLocation = new Point(500, 0);

                while (true)
                {
                    if (part == 1 && sandLocation.Y + 1 > maxY)
                    {
                        return field.Values.Count(cell => cell == 'o');
                    }
                    else if (part == 2 && field.TryGetValue(new(500, 0), out var startLocation) && startLocation == 'o')
                    {
                        return field.Values.Count(cell => cell == 'o');
                    }

                    if (IsFree(field, sandLocation.Add(Point.Down)))
                    {
                        sandLocation = sandLocation.Add(Point.Down);
                    }
                    else if (IsFree(field, sandLocation.Add(new(-1, 1))))
                    {
                        sandLocation = sandLocation.Add(new(-1, 1));
                    }
                    else if (IsFree(field, sandLocation.Add(new(1, 1))))
                    {
                        sandLocation = sandLocation.Add(new(1, 1));
                    }
                    else
                    {
                        field.Add(sandLocation, 'o');
                        break;
                    }
                }
            }
        }

        public static bool IsFree(Dictionary<Point, char> field, Point location)
        {
            if (field.TryGetValue(location, out var chr))
            {
                return chr == '.';
            }
            else
            {
                return true;
            }
        }

        public static Dictionary<Point, char> PrepareField(List<Path> paths, int part)
        {
            var field = new Dictionary<Point, char>();

            foreach (var path in paths)
            {
                foreach (var points in path.Points.SlidingWindow(2))
                {
                    DrawRock(field, points[0], points[1]);
                }
            }

            if (part == 2)
            {
                var bottomY = field.Keys.Max(p => p.Y) + 2;
                DrawRock(field, new(0, bottomY), new(1000, bottomY));
            }

            return field;
        }

        public static void DrawRock(Dictionary<Point, char> field, Point start, Point end)
        {
            foreach (var location in Shapes.Line(start, end))
            {
                field.TryAdd(location, '#');
            }
        }
    }
}
