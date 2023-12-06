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

            if (part1 != 1108800 || part2 != 36919753)
            {
                throw new Exception();
            }
        }

        public static long Part1(int[][] input)
        {
            var counts = new List<long>();
            var rounds = input[0].Zip(input[1]).ToArray();

            return rounds
                .Select(round => CalculateWinCount(round.First, round.Second))
                .Aggregate((x, y) => x * y);
        }

        public static int Part2(int[][] input)
        {
            var duration = JoinValues(input[0]);
            var highscore = JoinValues(input[1]);

            return CalculateWinCount((int)duration, highscore);
        }

        private static int CalculateWinCount(int duration, long highscore)
        {
            return Enumerable.Range(1, duration)
                .Count(chargeTime => Distance(chargeTime, duration) > highscore);
        }

        private static long Distance(long chargeTime, long roundDuration)
        {
            var remainingTime = roundDuration - chargeTime;
            return chargeTime * remainingTime;
        }

        private static long JoinValues(int[] input)
        {
            return long.Parse(input.Select(v => v.ToString()).Aggregate((x, y) => x + y));
        }
    }
}
