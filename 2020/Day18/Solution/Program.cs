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
        }

        public static long Part1(string[] input)
        {
            return input.Select(line => Part1Interpreter.Interpret(new Queue<char>(line.ToCharArray())))
                .Sum();
        }

        public static long Part2(string[] input)
        {
            return input.Select(line => Part2Interpreter.Interpret(new Queue<char>(line.ToCharArray())))
                .Sum();
        }
    }
}
