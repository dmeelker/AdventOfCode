using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Blizzard
    {
        public Point StartingLocation { get; }
        public Point Vector { get; }
        public int Range { get; }

        public Blizzard(Point location, Point vector, int range)
        {
            StartingLocation = location;
            Vector = vector;
            Range = range;
        }

        public Point GetLocationAtMinute(int minute)
        {
            return (Vector.X, Vector.Y) switch
            {
                (0, -1) => new(StartingLocation.X, Wrap(StartingLocation.Y - 1 - minute, Range) + 1),
                (0, 1) => new(StartingLocation.X, Wrap(StartingLocation.Y - 1 + minute, Range) + 1),
                (-1, 0) => new(Wrap(StartingLocation.X - 1 - minute, Range) + 1, StartingLocation.Y),
                (1, 0) => new(Wrap(StartingLocation.X - 1 + minute, Range) + 1, StartingLocation.Y),
                _ => throw new Exception()
            };
        }

        private int Wrap(int value, int max)
        {
            if (value < 0)
            {
                return max + (value % max) - 1;
            }
            else if (value >= max)
            {
                return value % max;
            }
            else
            {
                return value;
            }
        }
    }

    public class Input
    {
        public Grid<char> World { get; }
        public List<Blizzard> Blizzards { get; }

        public Input(Grid<char> world, List<Blizzard> blizzards)
        {
            World = world;
            Blizzards = blizzards;
        }
    }

    public static class Parser
    {
        public static Input ParseInput(string input)
        {
            var values = input.ToLines().Select(line => line.ToCharArray()).ToArray();

            var grid = CreateGrid(values);
            var blizzards = CreateBlizzards(values).ToList();
            return new(grid, blizzards);
        }

        private static Grid<char> CreateGrid(char[][] cells)
        {
            var width = cells[0].Length;
            var height = cells.Length;
            var grid = new Grid<char>(width, height, '.');

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var value = cells[y][x] switch
                    {
                        '#' => '#',
                        _ => '.'
                    };

                    grid.Set(x, y, value);
                }
            }

            return grid;
        }
        private static IEnumerable<Blizzard> CreateBlizzards(char[][] cells)
        {
            var width = cells[0].Length;
            var height = cells.Length;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var value = cells[y][x];

                    if (value == '>')
                    {
                        yield return new(new(x, y), Point.Right, width - 2);
                    }
                    else if (value == '<')
                    {
                        yield return new(new(x, y), Point.Left, width - 2);
                    }
                    else if (value == '^')
                    {
                        yield return new(new(x, y), Point.Up, height - 2);
                    }
                    else if (value == 'v')
                    {
                        yield return new(new(x, y), Point.Down, height - 2);
                    }
                }
            }
        }
    }
}
