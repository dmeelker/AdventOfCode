using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Solution
{
    [DebuggerDisplay("{Value}")]
    public class Number
    {
        public long Value { get; set; }

        public Number(long value)
        {
            Value = value;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static long Part1(List<int> input)
        {
            var valuesInOrder = input.Select(n => new Number(n)).ToList();
            var processingOrder = valuesInOrder.ToList();

            foreach (var value in processingOrder)
            {
                Move(value, valuesInOrder);
            }

            var resultValues = new[] {
                GetNthValue(valuesInOrder, 1000),
                GetNthValue(valuesInOrder, 2000),
                GetNthValue(valuesInOrder, 3000),
            };

            return resultValues.Sum(v => v);
        }
        public static long Part2(List<int> input)
        {
            var valuesInOrder = input.Select(n => new Number(n * 811_589_153L)).ToList();
            var processingOrder = valuesInOrder.ToList();

            for (var i = 0; i < 10; i++)
            {
                foreach (var value in processingOrder)
                {
                    Move(value, valuesInOrder);
                }
            }

            var resultValues = new[] {
                GetNthValue(valuesInOrder, 1000),
                GetNthValue(valuesInOrder, 2000),
                GetNthValue(valuesInOrder, 3000),
            };

            return resultValues.Sum(v => v);
        }

        public static void Move(Number value, List<Number> values)
        {
            var originalIndex = values.IndexOf(value);
            var newIndex = originalIndex + value.Value;
            values.RemoveAt(originalIndex);

            if (newIndex > values.Count)
            {
                newIndex = newIndex % values.Count;
            }
            else if (newIndex < 0)
            {
                newIndex = values.Count + (newIndex % values.Count);
            }
            else if (newIndex == 0 && value.Value < 0)
            {
                newIndex = values.Count;
            }
            else if (newIndex == values.Count && value.Value > 0)
            {
                newIndex = 0;
            }

            values.Insert((int)newIndex, value);
        }

        public static long GetNthValue(List<Number> value, int index)
        {
            var zeroValue = value.Single(v => v.Value == 0);
            var startIndex = value.IndexOf(zeroValue);

            var realIndex = (int)((startIndex + index) % value.Count);
            return value[realIndex].Value;
        }


    }
}
