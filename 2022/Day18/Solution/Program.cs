using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public record Point3(int X, int Y, int Z);

    public class Program
    {
        public const int SpaceMax = 23;

        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

            if (part1 != 4580 || part2 != 2610)
            {
                throw new Exception();
            }
        }

        public static int Part1(IEnumerable<Point3> input)
        {
            var points = new HashSet<Point3>(input);

            return input.Sum(point => Neighbours(point).Count(p => !points.Contains(p)));
        }

        public static int Part2(IEnumerable<Point3> input)
        {
            var points = new HashSet<Point3>(input.Select(p => Translate(p, new(1, 1, 1))));

            var visitedOutsidePoints = new HashSet<Point3>(Flood(new(0, 0, 0), points));

            return visitedOutsidePoints.Sum(p =>
                Neighbours(p).Count(n => points.Contains(n))
            );
        }

        public static IEnumerable<Point3> Neighbours(Point3 point)
        {
            yield return new(point.X, point.Y - 1, point.Z);
            yield return new(point.X, point.Y + 1, point.Z);

            yield return new(point.X - 1, point.Y, point.Z);
            yield return new(point.X + 1, point.Y, point.Z);

            yield return new(point.X, point.Y, point.Z - 1);
            yield return new(point.X, point.Y, point.Z + 1);
        }

        public static Point3 Translate(Point3 p, Point3 translation)
        {
            return new(p.X + translation.X, p.Y + translation.Y, p.Z + translation.Z);
        }

        public static IEnumerable<Point3> Flood(Point3 start, HashSet<Point3> points)
        {
            var open = new Queue<Point3>();
            var closed = new HashSet<Point3>();

            open.Enqueue(start);

            while (open.TryDequeue(out var currentLocation))
            {
                yield return currentLocation;

                var neighbours = Neighbours(currentLocation)
                    .Where(p =>
                        InSearchSpace(p) &&
                        !closed.Contains(p) &&
                        !open.Contains(p) &&
                        !points.Contains(p));

                foreach (var neighbour in neighbours)
                {
                    open.Enqueue(neighbour);
                }

                closed.Add(currentLocation);
            }
        }

        public static bool InSearchSpace(Point3 point)
        {
            return point.X >= 0 && point.X <= SpaceMax &&
                point.Y >= 0 && point.Y <= SpaceMax &&
                point.Z >= 0 && point.Z <= SpaceMax;
        }
    }
}
