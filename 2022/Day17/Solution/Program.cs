using System;
using System.IO;

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

            if (part1 != 3117L || part2 != 1553314121019L)
            {
                throw new Exception();
            }
        }

        public static long Part1(string input)
        {
            var simulator = new Simulator(input);
            return simulator.Simulate(2022, 1);
        }

        public static long Part2(string input)
        {
            var simulator = new Simulator(input);
            return simulator.Simulate(1_000_000_000_000, 2);
        }
    }
}
