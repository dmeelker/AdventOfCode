using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Sensor
    {
        public Point Location { get; set; }
        public Point BeaconLocation { get; set; }
        public int Range { get; private set; }
        public Point Corner { get; private set; }

        public Sensor(Point location, Point beaconLocation)
        {
            Location = location;
            BeaconLocation = beaconLocation;
            Range = beaconLocation.Subtract(Location).ManhattanDistance;
            Corner = location.Subtract(new(-Range, -Range));
        }

        public bool IsInRange(Point location)
        {
            var distance = location.Subtract(Location).ManhattanDistance;

            return distance <= Range;
        }

        public void Visualize()
        {
            var minX = Location.X - Range;
            var maxX = Location.X + Range;
            var minY = Location.Y - Range;
            var maxY = Location.Y + Range;

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    if (IsInRange(new(x, y)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public static class Parser
    {
        public static List<Sensor> ParseInput(string input)
        {
            return input.ToLines().Select(ParseLine).ToList();
        }

        private static Sensor ParseLine(string line)
        {
            var coordinatePairs = line.Replace("Sensor at ", "")
                .Replace(" closest beacon is at ", "")
                .Split(":");

            return new(ParsePoint(coordinatePairs[0]), ParsePoint(coordinatePairs[1]));
        }

        private static Point ParsePoint(string input)
        {
            input = input.Replace("x=", "").Replace("y=", "");
            var parts = input.ParseIntArray();
            return new(parts[0], parts[1]);
        }
    }
}
