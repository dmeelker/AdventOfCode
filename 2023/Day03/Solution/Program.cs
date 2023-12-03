using Shared;
using System;
using System.Collections.Generic;
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

        if (part1 != 550064 || part2 != 85010461)
        {
            throw new Exception();
        }
    }

    public static int Part1(Grid<char> input)
    {
        return FindNumbers(input)
            .Where(number => AdjacentToSymbol(number, input))
            .Sum(number => number.Value);
    }

    public static long Part2(Grid<char> input)
    {
        var gears = FindGears(input);
        var numbers = FindNumbers(input);

        return gears
            .Select(gear => numbers.Where(number => number.IsAdjacentTo(gear)).ToArray())
            .Where(adjacentNumbers => adjacentNumbers.Length == 2)
            .Sum(adjacentNumbers => adjacentNumbers[0].Value * adjacentNumbers[1].Value);
    }

    private static Number[] FindNumbers(Grid<char> input)
    {
        return input.Rows()
            .SelectMany(row =>
                row
                    .SequentialGroup(cell => char.IsDigit(cell.Value))
                    .Select(group => new Number(group[0].Location, GetNumber(group)))
            )
            .ToArray();
    }

    private static int GetNumber(IEnumerable<Grid<char>.CellReference<char>> cells)
    {
        return int.Parse(new string(cells.Select(cell => cell.Value).ToArray()));
    }

    private static Point[] FindGears(Grid<char> input)
    {
        return input.AllCells().Where(cell => cell.Value == '*')
            .Select(cell => cell.Location)
            .ToArray();
    }

    private static bool AdjacentToSymbol(Number number, Grid<char> grid)
    {
        return grid.RectangleOutline(number.Rect().Grow())
            .Any(cell => !char.IsDigit(cell.Value) && cell.Value != '.');
    }
}

public record Number(Point Location, int Value)
{
    public Rect Rect()
    {
        return new(Location.X, Location.Y, Value.ToString().Length, 1);
    }

    public bool IsAdjacentTo(Point Location)
    {
        return Rect().IsAdjacentTo(Location);
    }
};
