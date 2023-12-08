using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public record Input(string Directions, Node[] Nodes);
public record Node(string Name, string Left, string Right);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 16043 || part2 != 15726453850399L)
        {
            throw new Exception();
        }
    }

    public static int Part1(Input input)
    {
        var nodeLookup = input.Nodes.ToDictionary(n => n.Name);

        return FindPathLength("AAA", name => name == "ZZZ", nodeLookup, input.Directions);
    }

    public static long Part2(Input input)
    {
        var nodeLookup = input.Nodes.ToDictionary(n => n.Name);

        return input.Nodes.Where(n => n.Name.EndsWith("A"))
            .Select(node => (long)FindPathLength(node.Name, name => name.EndsWith("Z"), nodeLookup, input.Directions))
            .Aggregate(Arithmetic.LCM);
    }

    private static int FindPathLength(string startName, Func<string, bool> endNameFilter, Dictionary<string, Node> nodeLookup, string directions)
    {
        var currentNodeName = startName;
        var directionIndex = 0;
        var step = 0;

        while (!endNameFilter(currentNodeName))
        {
            var node = nodeLookup[currentNodeName];
            var direction = directions[directionIndex];

            currentNodeName = direction == 'L' ? node.Left : node.Right;

            directionIndex = (directionIndex + 1) % directions.Length;
            step++;
        }

        return step;
    }
}
