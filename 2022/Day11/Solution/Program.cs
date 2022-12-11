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
            var part1 = Part1(ParseInput());
            var part2 = Part2(ParseInput());

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

            if (part1 != 58056 || part2 != 15048718170)
            {
                throw new Exception();
            }
        }

        public static List<Monkey> ParseInput() => Parser.ParseInput(File.ReadAllText("input.txt"));

        public static long Part1(List<Monkey> monkeys) => Solve(monkeys, 20, false);
        public static long Part2(List<Monkey> monkeys) => Solve(monkeys, 10_000, true);

        private static long Solve(List<Monkey> monkeys, int rounds, bool part2)
        {
            var greatestDenominator = monkeys.Select(m => m.ThrowDivisor).Aggregate((x, y) => x * y);

            for (var i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.TryDequeue(out var item))
                    {
                        monkey.InspectCount++;
                        item = monkey.WorryFunction(item);

                        if (part2)
                        {
                            item = item % greatestDenominator;
                        }
                        else
                        {
                            item = (long)Math.Floor(item / 3.0);
                        }

                        monkeys[monkey.GetNextMonkeyId(item)].Items.Enqueue(item);
                    }
                }
            }

            return monkeys
                .Select(m => m.InspectCount)
                .OrderByDescending(v => v)
                .Take(2)
                .Aggregate((x, y) => x * y);
        }
    }
}
