using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Solution;

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Solve(input, false);
        var part2 = Solve(input, true);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 2094 || part2 != 6442)
        {
            throw new Exception();
        }
    }

    public static int Solve(Grid<char> input, bool canMoveUpSlopes)
    {
        var start = input.Row(0).Where(cell => cell.Value == '.').Single().Location;
        var end = input.Row(input.Height - 1).Where(cell => cell.Value == '.').Single().Location;

        var graph = ConstructGraph(input, start, canMoveUpSlopes);

        return FindLongestPath(graph[start], graph[end], graph);
    }

    private static Dictionary<Point, Node> ConstructGraph(Grid<char> grid, Point startLocation, bool canMoveUpSlopes)
    {
        var nodes = ConvertToGraph(grid, startLocation, canMoveUpSlopes);
        ConsolidateNodes(nodes);

        return nodes;
    }

    private static Dictionary<Point, Node> ConvertToGraph(Grid<char> grid, Point startLocation, bool canMoveUpSlopes)
    {
        var startNode = new Node(startLocation);
        var nodes = new Dictionary<Point, Node>() {
            {startLocation, startNode},
        };
        var open = new Queue<Node>();
        open.Enqueue(startNode);

        while (open.TryDequeue(out var node))
        {
            var options = grid.Neighbours(node.Location, false)
                .Where(option => CanMoveToCell(node.Location, option.Location, option.Value, canMoveUpSlopes));

            foreach (var option in options)
            {
                if (!nodes.TryGetValue(option.Location, out var neighbourNode))
                {
                    neighbourNode = new Node(option.Location);
                    nodes[option.Location] = neighbourNode;
                    open.Enqueue(neighbourNode);
                }

                node.EnsureConnected(neighbourNode, 1);
                if (CanMoveToCell(option.Location, node.Location, grid.Get(node.Location), canMoveUpSlopes))
                {
                    neighbourNode.EnsureConnected(node, 1);
                }
            }
        }

        return nodes;
    }

    private static void ConsolidateNodes(Dictionary<Point, Node> nodes)
    {
        while (nodes.Values.Any(node => IsContinuousPathNode(node, nodes)))
        {
            foreach (var node in nodes.Values.ToArray())
            {
                if (IsContinuousPathNode(node, nodes))
                {
                    nodes.Remove(node.Location);
                    node.Edges[0].Node.RemoveConnectionTo(node);
                    node.Edges[1].Node.RemoveConnectionTo(node);

                    node.Edges[0].Node.Connect(node.Edges[1].Node, node.Edges[0].Length + node.Edges[1].Length);
                }
            }
        }
    }

    private static bool IsContinuousPathNode(Node node, Dictionary<Point, Node> nodes)
    {
        if (node.Edges.Count != 2)
            return false;

        if (!node.Edges[0].Node.IsConnectedTo(node) || !node.Edges[1].Node.IsConnectedTo(node))
            return false;

        // If there are any other nodes connected to this one it cannot be removed
        if (nodes.Values.SelectMany(n => n.Edges).Count(edge => edge.Node == node) != 2)
            return false;

        return true;
    }

    private static int FindLongestPath(Node start, Node end, Dictionary<Point, Node> nodes)
    {
        var currentPath = new HashSet<Node>() {
            start
        };
        return FindLongestPath(start, end, currentPath, 0, nodes);
    }

    private static int FindLongestPath(Node startLocation, Node endLocation, HashSet<Node> currentPath, int pathLength, Dictionary<Point, Node> nodes)
    {
        if (startLocation == endLocation)
        {
            return pathLength;
        }

        var options = startLocation.Edges.Where(e => !currentPath.Contains(e.Node));
        int longestPath = 0;

        foreach (var option in options)
        {
            currentPath.Add(option.Node);
            var foundPath = FindLongestPath(option.Node, endLocation, currentPath, pathLength + option.Length, nodes);
            longestPath = Math.Max(foundPath, longestPath);
            currentPath.Remove(option.Node);
        }

        return longestPath;
    }

    private static bool CanMoveToCell(Point fromLocation, Point toLocation, char toChar, bool moveUpSlopes)
    {
        if (toChar == '#')
            return false;

        if (toChar == '.')
            return true;

        if (moveUpSlopes)
            return true;

        return toChar switch
        {
            '>' => toLocation == fromLocation.Add(Point.Right),
            '<' => toLocation == fromLocation.Add(Point.Left),
            'v' => toLocation == fromLocation.Add(Point.Down),
            _ => throw new Exception("Unknown slope type")
        };
    }
}

[DebuggerDisplay("{Location.X}, {Location.Y}")]
public class Node
{
    public Point Location { get; }
    public List<Edge> Edges { get; } = new();

    public Node(Point location)
    {
        Location = location;
    }

    public void EnsureConnected(Node other, int distance)
    {
        if (Edges.Any(e => e.Node == other))
            return;

        Edges.Add(new Edge(other, distance));
    }

    public void Connect(Node other, int distance)
    {
        Edges.Add(new Edge(other, distance));
        other.Edges.Add(new Edge(this, distance));
    }

    public void RemoveConnectionTo(Node other)
    {
        Edges.RemoveAll(e => e.Node == other);
    }

    public bool IsConnectedTo(Node other)
    {
        return Edges.Any(e => e.Node == other);
    }
}

public record Edge(Node Node, int Length);