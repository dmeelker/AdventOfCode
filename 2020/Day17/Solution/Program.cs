using System;
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

        public static int Part1(Field input)
        {
            foreach (var iteration in Enumerable.Range(0, 6))
            {
                input = Simulate(input, 3);
            }

            return input.CountActiveCells();
        }

        public static int Part2(Field input)
        {
            foreach (var iteration in Enumerable.Range(0, 6))
            {
                input = Simulate(input, 4);
            }

            return input.CountActiveCells();
        }

        public static Field Simulate(Field input, int dimensions)
        {
            input = dimensions == 4 ? input.Expand4d() : input.Expand3d();
            var result = input.Clone();

            input.VisitCells((point, value) => {
                var activeNeighbours = input.GetAdjacentCells(point).Select(p => input.Get(p)).Where(v => v).Count();

                if(value)
                {
                    if(activeNeighbours != 2 && activeNeighbours != 3)
                        result.Set(point, false);
                }
                else if(!value && activeNeighbours == 3)
                {
                    result.Set(point, true);
                }
            });

            return result;
        }
    }
}
