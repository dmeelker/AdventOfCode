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
            {
                return new string[] {
                    GetStringFromCells(input, cell.Location, new Point(1, 0), 4), // Right
                    GetStringFromCells(input, cell.Location, new Point(0, 1), 4), // Down
                    GetStringFromCells(input, cell.Location, new Point(-1, 1), 4), // BottomLeft
                    GetStringFromCells(input, cell.Location, new Point(1, 1), 4), // BottomRight
                }.Where(w => w.EqualsBothWays("XMAS")).Count();
            });
    }

    public static int Part2(Grid<char> input)
    {
        return input.AllCells()
            .Where(cell =>
            {
                return new string[] {
                    GetStringFromCells(input, cell.Location.Add(new Point(-1, -1)), new Point(1, 1), 3),
                    GetStringFromCells(input, cell.Location.Add(new Point(1, -1)), new Point(-1, 1), 3),
                }.All(w => w.EqualsBothWays("MAS"));
            })
            .Count();
    }

    private static string GetStringFromCells(Grid<char> grid, Point start, Point direction, int maxSteps)
    {
        var cells = grid.Path(start, direction, maxSteps);
        return string.Join(null, cells.Select(c => c.Value));
    }
}

