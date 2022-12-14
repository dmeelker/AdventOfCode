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
        }

        public static int Part1(List<Instruction> instructions)
        {
            var grid = new Grid<bool>(1000, 1000, false);

            foreach (var instruction in instructions)
            {
                switch (instruction.Action)
                {
                    case Action.TurnOn:
                        ExecuteInstruction(grid, instruction, (lightOn) => true);
                        break;
                    case Action.TurnOff:
                        ExecuteInstruction(grid, instruction, (lightOn) => false);
                        break;
                    case Action.Toggle:
                        ExecuteInstruction(grid, instruction, (lightOn) => !lightOn);
                        break;
                }
            }

            return grid.AllCells().Count(cell => cell.Value);
        }

        public static int Part2(List<Instruction> instructions)
        {
            var grid = new Grid<int>(1000, 1000, 0);

            foreach (var instruction in instructions)
            {
                switch (instruction.Action)
                {
                    case Action.TurnOn:
                        ExecuteInstruction(grid, instruction, (brightness) => brightness + 1);
                        break;
                    case Action.TurnOff:
                        ExecuteInstruction(grid, instruction, (brightness) => Math.Max(brightness - 1, 0));
                        break;
                    case Action.Toggle:
                        ExecuteInstruction(grid, instruction, (brightness) => brightness + 2);
                        break;
                }
            }

            return grid.AllCells().Sum(cell => cell.Value);
        }

        private static void ExecuteInstruction<T>(Grid<T> grid, Instruction instruction, Func<T, T> mutator) where T : IEquatable<T>
        {
            foreach (var point in Shapes.Rect(instruction.From, instruction.To.Subtract(instruction.From).Add(new(1, 1))))
            {
                var newValue = mutator(grid.Get(point));
                grid.Set(point, newValue);
            }
        }
    }
}
