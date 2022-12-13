using Shared;
using Shared.Dijkstra;
using System;
using System.IO;
using System.Linq;

namespace Solution
{
    public class HeightNode : IEquatable<HeightNode>
    {
        public Point Location;
        public char Height;

        public HeightNode(Point location, char height)
        {
            Location = location;
            Height = height;
        }

        public override int GetHashCode()
        {
            return Location.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is HeightNode node) return node.Location.Equals(Location);
            return false;
        }

        public bool Equals(HeightNode? other)
        {
            if (other == null) return false;
            return Location.Equals(other.Location);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));

            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

            if (part1 != 391 | part2 != 386)
            {
                throw new Exception();
            }
        }

        public static int Part1(Grid<char> input)
        {
            var startLocation = input.AllCells().First(cell => cell.Value == 'S').Location;
            var path = FindPath(input, startLocation);
            return path!.Steps.Count - 1;
        }

        public static int Part2(Grid<char> input)
        {
            var startLocations = input.AllCells()
                .Where(c => c.Value == 'S' || c.Value == 'a')
                .Where(c => input.Neighbours(c.Location, false).Any(n => n.Value == 'b'))
                .ToList();

            return startLocations
                .AsParallel()
                .Select((l, i) => FindPath(input, l.Location))
                .Where(path => path != null)
                .OrderBy(path => path!.Steps.Count)
                .First()
                .Steps.Count - 1;
        }

        private static Path<HeightNode>? FindPath(Grid<char> input, Point startLocation)
        {
            var grid = input.Map(cell => new SearchNode<HeightNode>(new(cell.Location, cell.Value)));

            var startNode = grid.Get(startLocation);
            var endNode = grid.AllCells().First(cell => cell.Value.Value.Height == 'E').Value;

            startNode.Value.Height = 'a';
            endNode.Value.Height = 'z';

            var searcher = new DijkstraSearcher<HeightNode>(
                grid.AllCells().Select(cell => cell.Value),
                startNode,
                endNode,
                (node) =>
                    grid.Neighbours(node.Value.Location, false)
                        .Select(cell => cell.Value)
                        .Where(n => n.Value.Height - node.Value.Height <= 1),
                (from, to) => 1
                );

            return searcher.FindPath();
        }
    }
}
