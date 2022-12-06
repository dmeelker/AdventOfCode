using System;
using System.IO;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static string[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static long Part1(string[] input)
        {
            var location = new Vector(0, 0);
            var vector = new Vector(1, 0);

            foreach (var line in input)
            {
                var action = line[0];
                var distance = long.Parse(line.Substring(1));

                switch (action)
                {
                    case 'N':
                        location.Y -= distance;
                        break;
                    case 'S':
                        location.Y += distance;
                        break;
                    case 'E':
                        location.X += distance;
                        break;
                    case 'W':
                        location.X -= distance;
                        break;
                    case 'L':
                        vector = vector.Rotate(-distance);
                        break;
                    case 'R':
                        vector = vector.Rotate(distance);
                        break;
                    case 'F':
                        location.X += vector.X * distance;
                        location.Y += vector.Y * distance;
                        break;
                }
            }

            return Math.Abs(location.X) + Math.Abs(location.Y);
        }

        public static int Part2(string[] input)
        {
            var waypointVector = new Vector(10, -1);
            var location = new Vector(0, 0);
            
            foreach (var line in input)
            {
                var action = line[0];
                var distance = long.Parse(line.Substring(1));

                switch (action)
                {
                    case 'N':
                        waypointVector.Y -= distance;
                        break;
                    case 'S':
                        waypointVector.Y += distance;
                        break;
                    case 'E':
                        waypointVector.X += distance;
                        break;
                    case 'W':
                        waypointVector.X -= distance;
                        break;
                    case 'L':
                        waypointVector = waypointVector.Rotate(-distance);
                        break;
                    case 'R':
                        waypointVector = waypointVector.Rotate(distance);
                        break;
                    case 'F':
                        location.X += waypointVector.X * distance;
                        location.Y += waypointVector.Y * distance;
                        break;
                }
            }

            return Math.Abs((int) location.X) + Math.Abs((int) location.Y);
        }
    }

    public class Vector
    {
        public long X { get; set; }
        public long Y { get; set; }

        public Vector(long x, long y)
        {
            X = x;
            Y = y;
        }

        public Vector Rotate(double degrees)
        {
            degrees = degrees * (Math.PI / 180);

            return new Vector(
                (long) Math.Round(X * Math.Cos(degrees) - (Y * Math.Sin(degrees))), 
                (long) Math.Round(X * Math.Sin(degrees) + (Y * Math.Cos(degrees)))
            );
        }
    }
}