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

        if (part1 != 6820 || part2 != 337)
        {
            throw new Exception();
        }
    }

    public static int Part1(Grid<char> input)
    {
        return TracePath(input).Count / 2;
    }

    private static List<Point> TracePath(Grid<char> input)
    {
        var start = input.AllCells().Single(c => c.Value == 'S');
        var visitedCells = new HashSet<Point>() { start.Location };

        var path = new List<Point>() {
            start.Location
        };

        do
        {
            var currentCell = path[^1];

            var possibleDirections =
                input.Neighbours(currentCell, false)
                .Where(neighbour => CanTravel(input, currentCell, neighbour.Location))
                .Where(neighbour => !visitedCells.Contains(neighbour.Location))
                .FirstOrDefault();

            if (possibleDirections == null)
            {
                break;
            }

            currentCell = possibleDirections.Location;
            path.Add(currentCell);
            visitedCells.Add(currentCell);
        } while (true);

        return path;
    }

    private static bool CanTravel(Grid<char> grid, Point from, Point to)
    {
        return FindPossibleDirections(grid, from).Contains(to) && FindPossibleDirections(grid, to).Contains(from);
    }

    private static IEnumerable<Point> FindPossibleDirections(Grid<char> grid, Point location)
    {
        var sourceChar = grid.Get(location);

        var directions = sourceChar switch
        {
            '|' => new[] { location.Add(new Point(0, -1)), location.Add(new Point(0, 1)) },
            '-' => new[] { location.Add(new Point(-1, 0)), location.Add(new Point(1, 0)) },
            'L' => new[] { location.Add(new Point(0, -1)), location.Add(new Point(1, 0)) },
            'J' => new[] { location.Add(new Point(0, -1)), location.Add(new Point(-1, 0)) },
            '7' => new[] { location.Add(new Point(0, 1)), location.Add(new Point(-1, 0)) },
            'F' => new[] { location.Add(new Point(0, 1)), location.Add(new Point(1, 0)) },
            'S' => new[] { location.Add(new Point(0, -1)), location.Add(new Point(0, 1)), location.Add(new Point(-1, 0)), location.Add(new Point(1, 0)) },
            _ => Array.Empty<Point>()
        };

        return grid.Neighbours(location, false)
            .Where(cell => directions.Contains(cell.Location))
            .Select(cell => cell.Location);
    }

    public static int Part2(Grid<char> grid)
    {
        var path = TracePath(grid);

        grid = grid.Clone();
        ReplaceStart(grid, path);
        ReplaceNonPathTiles(grid, path);

        var expanded = ExpandGrid(grid);

        var randomSegment = path.First(l => grid.Get(l) == '|').Multiply(3);
        Flood(expanded, randomSegment.Add(new Point(0, 1)));

        var marked = expanded.AllCells().Where(c => c.Location.X % 3 == 1 && c.Location.Y % 3 == 1 && c.Value == '#').Count();
        var unmarked = expanded.AllCells().Where(c => c.Location.X % 3 == 1 && c.Location.Y % 3 == 1 && c.Value == '.').Count();

        return Math.Min(marked, unmarked);
    }

    private static void ReplaceNonPathTiles(Grid<char> grid, List<Point> path)
    {
        var occupiedCells = path.ToHashSet();

        grid.AllCells()
            .Where(cell => !occupiedCells.Contains(cell.Location))
            .ToList()
            .ForEach(cell => cell.Value = '.');
    }

    private static void ReplaceStart(Grid<char> grid, List<Point> path)
    {
        var v1 = path[1].Subtract(path[0]);
        var v2 = path[^1].Subtract(path[0]);

        if (v1.Y == 1 && v2.X == 1)
        {
            grid.Set(path[0], 'F');
        }

        if (v1.Y == -1 && v2.Y == 1)
        {
            grid.Set(path[0], '|');
        }

        if (v1.X == -1 && v2.Y == 1)
        {
            grid.Set(path[0], '7');
        }
    }

    private static Grid<char> ExpandGrid(Grid<char> input)
    {
        var expanded = new Grid<char>(input.Width * 3, input.Height * 3, '.');

        foreach (var cell in input.AllCells())
        {
            var expandedCell = ExpandCell(cell.Value);

            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    expanded.Set(cell.Location.X * 3 + x, cell.Location.Y * 3 + y, expandedCell[y, x]);
                }
            }
        }

        return expanded;
    }

    private static char[,] ExpandCell(char value)
    {
        if (value == '|')
        {
            return new[,] {
                { '.', '|', '.' },
                { '.', '|', '.' },
                { '.', '|', '.' }
            };
        }
        else if (value == '-')
        {
            return new[,] {
                { '.', '.', '.' },
                { '-', '-', '-' },
                { '.', '.', '.' }
            };
        }
        else if (value == 'F')
        {
            return new[,] {
                { '.', '.', '.' },
                { '.', 'F', 'F' },
                { '.', 'F', '.' }
            };
        }
        else if (value == '7')
        {
            return new[,] {
                { '.', '.', '.' },
                { '7', '7', '.' },
                { '.', '7', '.' }
            };
        }
        else if (value == 'L')
        {
            return new[,] {
                { '.', 'L', '.' },
                { '.', 'L', 'L' },
                { '.', '.', '.' }
            };
        }
        else if (value == 'J')
        {
            return new[,] {
                { '.', 'J', '.' },
                { 'J', 'J', '.' },
                { '.', '.', '.' }
            };
        }
        else if (value == 'S')
        {
            return new[,] {
                { 'S', 'S', 'S' },
                { 'S', 'S', 'S' },
                { 'S', 'S', 'S' }
            };
        }

        return new[,] {
            { '.', '.', '.' },
            { '.', '.', '.' },
            { '.', '.', '.' }
        };
    }

    private static void Flood(Grid<char> input, Point location)
    {
        input.Flood(location).ToList().ForEach(cell => cell.Value = '#');
    }
}