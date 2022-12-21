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

        public static int Part1(List<Sensor> sensors)
        {
            var minX = sensors.Min(s => s.Location.X - s.Range);
            var maxX = sensors.Max(s => s.Location.X + s.Range);
            var y = 2_000_000;

            return Enumerable.Range(minX, maxX - minX)
                .Select(x => new Point(x, y))
                .Count(location => sensors.Any(sensor => !location.Equals(sensor.BeaconLocation) && sensor.IsInRange(location)));
        }

        public static long Part2(List<Sensor> sensors)
        {
            int max = 4_000_000;

            var location = sensors.SelectMany(TraceSensorReach)
                .AsParallel()
                .Distinct()
                .Where(location => location.X >= 0 && location.X <= max && location.Y >= 0 && location.Y <= max)
                .Single(location => !sensors.Any(sensor => sensor.IsInRange(location)));

            return (location.X * 4_000_000L) + location.Y;
        }

        public static IEnumerable<Point> TraceSensorReach(Sensor sensor)
        {
            var range = sensor.Range + 1;
            var minX = sensor.Location.X - range;
            var maxX = sensor.Location.X + range;

            yield return new(sensor.Location.Y, minX);
            yield return new(sensor.Location.Y, maxX);

            for (var y = 1; y <= range; y++)
            {
                var xOffset = y;
                var width = ((range * 2) + 1) - (y * 2);

                yield return new(sensor.Location.Y + y, minX + xOffset);
                yield return new(sensor.Location.Y + y, minX + xOffset + width - 1);
                yield return new(sensor.Location.Y - y, minX + xOffset);
                yield return new(sensor.Location.Y - y, minX + xOffset + width - 1);
            }
        }
    }
}
