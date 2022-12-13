using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    record Path(Point startLocation, List<SearchNode> steps);

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
            return path.steps.Count - 1;
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
                .OrderBy(path => path.steps.Count)
                .First()
                .steps.Count - 1;
        }

        private static Path? FindPath(Grid<char> input, Point startLocation)
        {
            var arr = new SearchNode[input.Width, input.Height];
            for (var x = 0; x < input.Width; x++)
            {
                for (var y = 0; y < input.Height; y++)
                {
                    arr[x, y] = new SearchNode()
                    {
                        Location = new Point(x, y),
                        Height = input.Get(x, y),
                        Distance = int.MaxValue,
                        Visited = false
                    };
                }
            }

            var grid = new Grid<SearchNode>(arr);

            var startNode = grid.Get(startLocation);
            var endNode = grid.AllCells().First(cell => cell.Value.Height == 'E').Value;

            grid.AllCells().First(c => c.Value.Height == 'S').Value.Height = 'a';

            startNode.Distance = 0;
            endNode.Height = 'z';

            var closed = new HashSet<SearchNode>();
            var open = new List<SearchNode>();


            open.AddRange(grid.AllCells().Select(cell => cell.Value));
            open.Sort((node1, node2) => node1.Distance.CompareTo(node2.Distance));


            while (open.Count > 0)
            {
                var currentNode = open[0];
                if (currentNode.Distance == int.MaxValue)
                    break;

                var changes = false;

                open.RemoveAt(0);

                var neighbours = grid.Neighbours(currentNode.Location, false)
                    .Where(n => !closed.Contains(n.Value))
                    .Where(n => n.Value.Height - currentNode.Height <= 1).ToList();

                foreach (var neighbour in neighbours)
                {
                    var newDistance = currentNode.Distance + 1;
                    if (newDistance < neighbour.Value.Distance)
                    {
                        neighbour.Value.Distance = newDistance;
                        neighbour.Value.Source = currentNode;
                        changes = true;
                    }
                }

                closed.Add(currentNode);
                if (currentNode.Equals(endNode))
                {
                    break;
                }

                if (changes)
                    open = open.Where(n => n.Distance != int.MaxValue)
                        .OrderBy(n => n.Distance)
                        .Concat(open.Where(n => n.Distance == int.MaxValue))
                        .ToList();
            }

            if (endNode.Source == null)
            {
                // No Path
                return null;
            }

            var path = new List<SearchNode>();
            {
                var currentNode = endNode;
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.Source;
                }

                path.Reverse();
            }

            return new(startLocation, path);
        }
    }

    class SearchNode : IEquatable<SearchNode>
    {
        public Point Location { get; set; }
        public char Height { get; set; }
        public bool Visited { get; set; }
        public int Distance { get; set; } = int.MaxValue;
        public SearchNode? Source { get; set; } = null;

        public override int GetHashCode()
        {
            return Location.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SearchNode node) return node.Location.Equals(Location);
            return false;
        }

        public bool Equals(SearchNode other)
        {
            return Location.Equals(other.Location);
        }
    }
}
