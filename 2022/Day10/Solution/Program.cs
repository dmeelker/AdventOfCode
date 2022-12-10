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
            var input = Parser.ParseInput(File.ReadAllText("input.txt")).ToList();
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: ");
            PrintBuffer(part2);

            if (part1 != 15880 || part2 != "###..#.....##..####.#..#..##..####..##..#..#.#....#..#.#....#.#..#..#....#.#..#.#..#.#....#....###..##...#..#...#..#....###..#....#.##.#....#.#..####..#...#.##.#....#....#..#.#....#.#..#..#.#....#..#.#....####..###.#....#..#.#..#.####..###.")
            {
                throw new Exception();
            }
        }

        public static long Part1(IEnumerable<Operation> input)
        {
            var cycle = 0;
            var x = 1;
            var score = 0L;

            foreach (var operation in input)
            {
                if (operation.Name == "noop")
                {
                    Cycle(1);
                }
                else if (operation.Name == "addx")
                {
                    Cycle(2);
                    x += operation.Parameter.Value;
                }
            }

            return score;

            void Cycle(int quantity)
            {
                for (var i = 0; i < quantity; i++)
                {
                    cycle++;

                    if ((cycle - 20) % 40 == 0)
                    {
                        score += x * cycle;
                    }
                }
            }
        }

        public static string Part2(IEnumerable<Operation> input)
        {
            var outputBuffer = Enumerable.Repeat('.', 40 * 6).ToArray(); ;

            var cycle = 0;
            var x = 1;

            foreach (var operation in input)
            {
                if (operation.Name == "noop")
                {
                    Cycle(1);
                }
                else if (operation.Name == "addx")
                {
                    Cycle(2);
                    x += operation.Parameter.Value;
                }
            }
            return string.Concat(outputBuffer);

            void Cycle(int quantity)
            {
                for (var i = 0; i < quantity; i++)
                {
                    cycle++;

                    var drawX = (cycle - 1) % 40;
                    if (drawX >= x - 1 && drawX <= x + 1)
                    {
                        outputBuffer[cycle - 1] = '#';
                    }
                }
            }
        }

        private static void PrintBuffer(string outputBuffer)
        {
            for (var i = 0; i < outputBuffer.Length; i++)
            {
                if (i > 0 && i % 40 == 0)
                {
                    Console.WriteLine();
                }
                Console.Write(outputBuffer[i]);
            }
        }
    }
}
