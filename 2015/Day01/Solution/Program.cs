using System;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(string input)
        {
            return input.ToCharArray().Sum(chr => chr switch
            {
                '(' => 1,
                ')' => -1
            });
        }

        public static int Part2(string input)
        {
            var values = input.ToCharArray().Select(chr => chr switch
            {
                '(' => 1,
                ')' => -1
            });

            values.Aggregate()

            var sum = 0;
            var index = 0;
            foreach (var value in values)
            {
                sum += value;
                if (sum < 0)
                {
                    return index + 1;
                }
                index++;
            }

            return -1;
        }
    }
}
