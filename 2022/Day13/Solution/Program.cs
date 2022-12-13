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

            if (part1 != 5623 || part2 != 20570)
            {
                throw new Exception();
            }
        }

        public static int Part1(List<ValuePair> input)
        {
            var comparer = new ValueComparer();

            return input
                .Select((value, index) => comparer.Compare(value.Left, value.Right) == -1 ? index + 1 : 0)
                .Sum();
        }

        public static int Part2(List<ValuePair> input)
        {
            var dividerPackets = new Value[] {
                new ListValue(new ListValue(new NumericValue(2))),
                new ListValue(new ListValue(new NumericValue(6))),
            };

            var packets = input.SelectMany(p => new[] { p.Left, p.Right })
                .Concat(dividerPackets)
                .OrderBy(packet => packet, new ValueComparer())
                .ToList();

            return (packets.IndexOf(dividerPackets[0]) + 1) * (packets.IndexOf(dividerPackets[1]) + 1);
        }
    }

    public class ValueComparer : IComparer<Value>
    {
        public int Compare(Value left, Value right)
        {
            if (left is NumericValue leftN && right is NumericValue rightN)
            {
                return CompareNumericValues(leftN, rightN);
            }
            else
            {
                return CompareListValues(ToList(left), ToList(right));
            }
        }

        public ListValue ToList(Value value)
        {
            if (value is ListValue listValue)
            {
                return listValue;
            }
            else
            {
                return new ListValue(value);
            }
        }

        public int CompareListValues(ListValue left, ListValue right)
        {
            var count = Math.Max(left.Values.Count, right.Values.Count);
            for (var i = 0; i < count; i++)
            {
                if (i >= left.Values.Count)
                {
                    return -1;
                }

                if (i >= right.Values.Count)
                {
                    return 1;
                }

                var result = Compare(left.Values[i], right.Values[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        public int CompareNumericValues(NumericValue left, NumericValue right)
        {
            return left.Value.CompareTo(right.Value);
        }
    }
}
