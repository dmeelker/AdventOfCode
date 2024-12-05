using Shared;
using System;
using System.IO;
using System.Linq;

namespace Solution;

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 2583 || part2 != 1978)
        {
            throw new Exception("Test failed");
        }
    }

    public static int Part1(Grid<char> input)
    {
        return input.AllCells()
            .Sum(cell =>
                new string[] {
                    GetStringFromCells(input, cell.Location, Point.Right, 4),
                    GetStringFromCells(input, cell.Location, Point.Down, 4),
                    GetStringFromCells(input, cell.Location, Point.DownLeft, 4),
                    GetStringFromCells(input, cell.Location, Point.DownRight, 4),
                }.Count(w => w.EqualsBothWays("XMAS"))
            );
    }

    public static int Part2(Grid<char> input)
    {
        return input.AllCells()
            .Count(cell =>
                new string[] {
                    GetStringFromCells(input, cell.Location + Point.UpLeft, Point.DownRight, 3),
                    GetStringFromCells(input, cell.Location + Point.UpRight, Point.DownLeft, 3),
                }.All(w => w.EqualsBothWays("MAS"))
            );
    }

    private static string GetStringFromCells(Grid<char> grid, Point start, Point direction, int maxSteps)
    {
        var cells = grid.Path(start, direction, maxSteps);
        return string.Join(null, cells.Select(c => c.Value));
    }
}

