using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Solution
{
    [DebuggerDisplay("{FromX},{ToX} {Y}")]
    public class Slice
    {
        public int Y { get; set; }
        public int FromX { get; set; }
        public int ToX { get; set; }

        public Slice(int y, int from, int to)
        {
            Y = y;
            FromX = from;
            ToX = to;
        }

        public bool Overlaps(Slice other)
        {
            if (other.Y != Y)
                return false;

            return !(other.FromX > ToX || other.ToX < FromX);
        }

        public Slice Merge(Slice other)
        {
            if (!Overlaps(other) && !Adjacent(other))
                throw new Exception();

            return new Slice(Y, Math.Min(FromX, other.FromX), Math.Max(ToX, other.ToX));
        }

        public bool Adjacent(Slice other)
        {
            if (other.Y != Y)
                return false;

            return ToX == other.FromX - 1 || FromX == other.ToX + 1;
        }

        public Slice Clamp(int min, int max)
        {
            return new(Y, Math.Max(FromX, min), Math.Min(ToX, max));
        }
    }

    // 5129026 low
    // 5181556
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
            return 0;
            foreach (var sensor in sensors)
            {
                Console.WriteLine($"{sensor.Location} ({sensor.Range}) {sensor.Location.X - sensor.Range}");
            }

            var minX = sensors.Min(s => s.Location.X - s.Range);
            var maxX = sensors.Max(s => s.Location.X + s.Range);
            var count = 0;
            var y = 2_000_000;

            for (var x = minX; x < maxX; x++)
            {
                var location = new Point(x, y);

                if (sensors.Any(sensor => !location.Equals(sensor.BeaconLocation) && sensor.IsInRange(location)))
                {
                    count++;
                }

                //bool hit = false;
                //foreach (var sensor in sensors)
                //{
                //    if (!location.Equals(sensor.BeaconLocation) && sensor.IsInRange(location))
                //    {
                //        Console.Write("#");
                //        count++;
                //        hit = true;
                //        break;
                //    }

                //}
                //if (!hit)
                //    Console.Write(".");
            }

            return count;
        }
        public static long GetTime()
        {
            return DateTime.Now.Ticks;
        }
        public static long Part2(List<Sensor> sensors)
        {
            int max = 4_000_000;

            var start = GetTime();

            Console.WriteLine("Slicing");
            var allSlices = sensors.AsParallel()
                .SelectMany(SliceSensorReach)
                .Where(slice => slice.Y >= 0 && slice.Y <= max)
                .Where(slice => !(slice.FromX > max | slice.ToX < 0))
                .Select(slice => slice.Clamp(0, max))
                .ToArray();
            Console.WriteLine($"Slicing done ({GetTime() - start})");

            Console.WriteLine("Grouping");
            var slicesPerLayer = allSlices
                .GroupBy(slice => slice.Y)
                .ToDictionary(group => group.Key, group => group.ToList());
            Console.WriteLine($"Grouping done ({GetTime() - start})");

            Console.WriteLine("Merging");
            foreach (var y in slicesPerLayer.Keys)
            {
                var layer = slicesPerLayer[y];
                var merged = false;
                do
                {
                    merged = false;
                    for (var i = 0; i < layer.Count; i++)
                    {
                        for (var j = 0; j < layer.Count; j++)
                        {
                            if (j == i)
                                continue;

                            var slices = new[] { layer[i], layer[j] };

                            if (slices[0].Overlaps(slices[1]) || slices[0].Adjacent(slices[1]))
                            {
                                var mergedLayer = slices[0].Merge(slices[1]);
                                layer.Remove(slices[0]);
                                layer.Remove(slices[1]);
                                layer.Add(mergedLayer);
                                i--;
                                merged = true;
                                break;
                            }
                        }
                    }

                } while (merged);
            }
            Console.WriteLine($"Merging done ({GetTime() - start})");

            Console.WriteLine("Gather results");
            var notCovered = slicesPerLayer.Where(group => group.Value.Count > 1).Select(group => group.Value).Single();
            var resultSlices = notCovered.OrderBy(slice => slice.FromX).ToList();

            var result = new Point(resultSlices[0].ToX + 1, resultSlices[0].Y);

            return (result.X * 4_000_000L) + result.Y;
        }

        public static IEnumerable<Slice> SliceSensorReach(Sensor sensor)
        {
            var minX = sensor.Location.X - sensor.Range;
            var maxX = sensor.Location.X + sensor.Range;
            var minY = sensor.Location.Y - sensor.Range;

            yield return new(sensor.Location.Y, minX, maxX);

            for (var y = 1; y <= sensor.Range; y++)
            {
                var xOffset = y;
                var width = ((sensor.Range * 2) + 1) - (y * 2);
                yield return new(sensor.Location.Y + y, minX + xOffset, minX + xOffset + width - 1);
                yield return new(sensor.Location.Y - y, minX + xOffset, minX + xOffset + width - 1);
            }
        }
    }
}
