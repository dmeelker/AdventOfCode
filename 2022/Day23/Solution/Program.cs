using Shared;
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

            if (part1 != 4068 || part2 != 968)
            {
                throw new Exception();
            }
        }

        public static int Part1(List<Point> input)
        {
            var simulator = new ElfSimulator(input.Select(p => new Elf(p)));

            for (var round = 0; round < 10; round++)
            {
                simulator.SimulateRound();
            }

            return CountEmptySpots(simulator);
        }

        private static int CountEmptySpots(ElfSimulator simulator)
        {
            var boundary = GetElfBoundary(simulator.Elves);
            var elfLocations = new HashSet<Point>(simulator.Elves.Select(e => e.Location));
            return Shapes.Rect(boundary).Count(location => !elfLocations.Contains(location));
        }

        public static int Part2(List<Point> input)
        {
            var simulator = new ElfSimulator(input.Select(p => new Elf(p)));
            int round = 1;

            while (true)
            {
                if (simulator.SimulateRound() == 0)
                {
                    return round;
                }
                round++;
            }
        }

        public static Rect GetElfBoundary(IEnumerable<Elf> elves)
        {
            var minX = elves.Min(elf => elf.Location.X);
            var maxX = elves.Max(elf => elf.Location.X);
            var minY = elves.Min(elf => elf.Location.Y);
            var maxY = elves.Max(elf => elf.Location.Y);

            return new(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }
    }
}
