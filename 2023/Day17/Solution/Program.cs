using Shared;
using Shared.Dijkstra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public class Node : SearchNode<Point>
{
    public Node(Point value) : base(value)
    {
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("testinput.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
    }

    public static int Part1(Grid<int> input)
    {
        var map = ToNodeGrid(input);
        var allNodes = map.AllCells().Select(c => c.Value).ToArray();
        var startNode = allNodes.First(n => n.Value == new Point(0, 0));
        var endNode = allNodes.First(n => n.Value == new Point(input.Width - 1, input.Height - 1));

        var searcher = new DijkstraSearcher<Point>(
            allNodes,
            startNode,
            endNode,
            node =>
            {

                return map.Neighbours(node.Value, false).Select(cell => cell.Value);
            },
            (from, to) =>
            {
                if (IsStraightLine(from, to))
                    return 1000;

                return input.Get(to.Value);
            }
        );

        var path = searcher.FindPath();

        Print(input, path!);

        return path!.Steps.Skip(1).Sum(n => input.Get(n.Value));
    }

    private static bool IsStraightLine(SearchNode<Point> from, SearchNode<Point> to)
    {
        //var locations = from.TraceBack(3)
        //    .Reverse()
        //    .Select(n => n.Value)
        //    .Union(new[] { to.Value })
        //    .ToArray(); ;

        if (from.Source == null || from.Source.Source == null)
            return false;

        var locations = new[] { from.Source.Source.Value, from.Source.Value, from.Value, to.Value };

        return locations.All(l => l.X == locations[0].X) || locations.All(l => l.Y == locations[0].Y);
    }

    private static void Print(Grid<int> input, Path<Point> path)
    {
        var pathLocations = path.Steps.Select(n => n.Value).ToHashSet();

        for (var y = 0; y < input.Height; y++)
        {
            for (var x = 0; x < input.Width; x++)
            {
                if (pathLocations.Contains(new Point(x, y)))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }

                Console.Write(input.Get(x, y));
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
    }

    private static Grid<SearchNode<Point>> ToNodeGrid(Grid<int> input)
    {
        var clone = new Grid<SearchNode<Point>>(input.Width, input.Height);

        for (var x = 0; x < input.Width; x++)
        {
            for (var y = 0; y < input.Height; y++)
            {
                clone.Set(x, y, new SearchNode<Point>(new Point(x, y)));
            }
        }

        return clone;
    }

    private static IEnumerable<SearchNode<Point>> FindOptions(Grid<int> map, SearchNode<Point> node)
    {
        return map.Neighbours(node.Value, false)
            .Select(location => new SearchNode<Point>(location.Location));
    }

    private static int Cost(Grid<int> map, SearchNode<Point> from, SearchNode<Point> to)
    {
        var toCell = map.Get(to.Value);
        return toCell;
    }


    public static int Part2(Grid<int> input)
    {
        return 0;
    }
}
