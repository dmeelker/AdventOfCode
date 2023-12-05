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

            if (part1 != 309796150 || part2 != 50716416)
            {
                throw new Exception();
            }
        }

        public static long Part1(Input input)
        {
            var values = input.Seeds;

            foreach (var mapping in input.Mappings)
            {
                values = Map(values, mapping);
            }

            return values.OrderBy(v => v).First();
        }

        private static long[] Map(long[] values, Mapping mapping)
        {
            var output = new List<long>();

            foreach (var value in values)
            {
                var mapped = false;
                foreach (var range in mapping.Ranges)
                {
                    if (range.Source.Contains(value))
                    {
                        output.Add(range.Destination.Start + (value - range.Source.Start));
                        mapped = true;
                        break;
                    }
                }

                if (!mapped)
                    output.Add(value);
            }

            return output.ToArray();
        }

        public static long Part2(Input input)
        {
            var ranges = input.Seeds.ToPairs().Select(pair => new Range(pair.Item1, pair.Item2)).ToArray();

            foreach (var mapping in input.Mappings)
            {
                ranges = Map(ranges, mapping);
            }

            return ranges.OrderBy(v => v.Start).First().Start;
        }

        private static Range[] Map(Range[] ranges, Mapping mapping)
        {
            var remainingRanges = new List<Range>(ranges);
            var intersectedRanges = new List<Range>();

            foreach (var mappingRange in mapping.Ranges)
            {
                foreach (var range in remainingRanges.ToArray())
                {
                    var intersection = range.Intersect(mappingRange.Source);
                    if (intersection != null)
                    {
                        remainingRanges.Remove(range);
                        intersectedRanges.Add(intersection.Intersection.Transpose(mappingRange.Offset));
                        remainingRanges.AddRange(intersection.Remainder);
                    }
                }
            }

            return intersectedRanges.Concat(remainingRanges).ToArray();
        }
    }
}
