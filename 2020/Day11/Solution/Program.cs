using System;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(char[,] input)
        {
            return Solve(input, Simulator.SimulatePart1);
        }

        public static int Part2(char[,] input)
        {
            return Solve(input, Simulator.SimulatePart2);
        } 

        public static int Solve(char[,] input, Func<char[,], char[,]> simulator)
        {
            var state = input;
            var nextState = input;

            do
            {
                state = nextState;
                nextState = simulator(state);
            }
            while (!state.IsEqual(nextState));

            return nextState.CountCells(chr => chr == '#');
        }

        public static char[,] ParseInput(string input)
        {
            var parseResult = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToCharArray())
                .ToArray();

            var result = new char[parseResult.Length, parseResult[0].Length];

            for (var x = 0; x < result.GetLength(0); x++)
            {
                for (var y = 0; y < result.GetLength(1); y++)
                {
                    result[x, y] = parseResult[x][y];
                }
            }

            return result;
        }
    }
}
