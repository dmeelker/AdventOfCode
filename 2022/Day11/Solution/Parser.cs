using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Monkey
    {
        public required Queue<long> Items { get; set; } = new();
        public required Func<long, long> WorryFunction { get; set; } = (x) => x;
        public required long ThrowDivisor { get; set; }
        public required int TrueMonkeyId { get; set; }
        public required int FalseMonkeyId { get; set; }
        public long InspectCount { get; set; } = 0;

        public int GetNextMonkeyId(long item)
        {
            return item % ThrowDivisor == 0 ? TrueMonkeyId : FalseMonkeyId;
        }
    }

    public static class Parser
    {
        public static List<Monkey> ParseInput(string input)
        {
            return input.ToSections().Select(ParseSection).ToList();
        }

        private static Monkey ParseSection(string section)
        {
            var lines = section.ToLines();

            return new()
            {
                Items = new(lines[1].StripPrefix("  Starting items: ").ParseLongArray()),
                WorryFunction = ParseWorryFunction(lines[2].StripPrefix("  Operation: new = ")),
                ThrowDivisor = lines[3].StripPrefix("  Test: divisible by ").ParseLong(),
                TrueMonkeyId = lines[4].StripPrefix("    If true: throw to monkey ").ParseInt(),
                FalseMonkeyId = lines[5].StripPrefix("    If false: throw to monkey ").ParseInt()
            };
        }

        private static Func<long, long> ParseWorryFunction(string input)
        {
            if (input == "old * old")
            {
                return (x) => x * x;
            }
            else if (input.StartsWith("old + "))
            {
                var value = long.Parse(input.Substring("old + ".Length));
                return (x) => x + value;
            }
            else if (input.StartsWith("old * "))
            {
                var value = long.Parse(input.Substring("old * ".Length));
                return (x) => x * value;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
