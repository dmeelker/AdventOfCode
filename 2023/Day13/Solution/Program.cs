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
        var part1 = Solve(input, 0);
        var part2 = Solve(input, 1);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        if (part1 != 29846 || part2 != 25401)
        {
            throw new Exception();
        }
    }

    private static int Solve(Grid<char>[] input, int differences)
    {
        return input.Sum(grid =>
            FindReflection(GridColumns(grid), differences) ??
            (FindReflection(GridRows(grid), differences)!.Value * 100)
        );
    }

    private static int? FindReflection(ICollection<ICollection<char>> lines, int expectedDifferences)
    {
        for (var i = 1; i < lines.Count; i++)
        {
            var maxSize = Math.Min(i, lines.Count - i);
            var differences = Enumerable.Range(1, maxSize)
                .Sum(offset => CheckReflection(lines, i, offset));

            if (differences == expectedDifferences)
            {
                return i;
            }
        }

        return null;
    }

    private static int CheckReflection(ICollection<ICollection<char>> lines, int mirrorIndex, int offset)
    {
        return CountDifferences(
            lines.ElementAt(mirrorIndex + offset - 1),
            lines.ElementAt(mirrorIndex - offset));
    }

    private static int CountDifferences(IEnumerable<char> line1, IEnumerable<char> line2)
    {
        return line1.Zip(line2)
            .Count(pair => pair.First != pair.Second);
    }

    private static char[][] GridRows(Grid<char> grid) => grid.RowValues().Select(v => v.ToArray()).ToArray();
    private static char[][] GridColumns(Grid<char> grid) => grid.ColumnValues().Select(v => v.ToArray()).ToArray();
}
