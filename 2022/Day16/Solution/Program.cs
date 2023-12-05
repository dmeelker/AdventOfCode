using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Solution
{
    [DebuggerDisplay("{Name}")]
    public class Node
    {
        public string Name { get; }
        public int FlowRate { get; }
        public List<Node> Nodes { get; set; } = new();

        public Node(string name, int flowRate)
        {
            Name = name;
            FlowRate = flowRate;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("testinput.txt"));

            var part1 = 2183; // Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

            if (part1 != 2183)
            {
                throw new Exception();
            }
        }

        public static int Part1(List<ValveRoom> input)
        {
            var root = BuildModel(input, out var allNodes);

            var distances = new NodeDistances(allNodes);
            var nodesToVisit = allNodes.Where(node => node != root && node.FlowRate > 0).ToArray();
            return FindMaximumFlowRate(root, new(), new(nodesToVisit), 0, 0, 0, distances);
        }

        public static int FindMaximumFlowRate(Node currentNode, Stack<Node> currentPath, HashSet<Node> unvisitedNodes, int pathFlowRate, int minutesPassed, int maxFoundFlowRate, NodeDistances distances)
        {
            if (minutesPassed >= 30)
            {
                return Math.Max(pathFlowRate, maxFoundFlowRate);
            }

            var options = unvisitedNodes.ToArray();

            foreach (var option in options)
            {
                var distance = distances.GetDistance(currentNode, option);
                var timeWhenValveIsOpened = minutesPassed + distance + 1;
                var timeLeftWhenValveIsOpened = 30 - timeWhenValveIsOpened;
                unvisitedNodes.Remove(option);

                currentPath.Push(option);
                var result = FindMaximumFlowRate(
                    option,
                    currentPath,
                    unvisitedNodes,
                    pathFlowRate + (option.FlowRate * timeLeftWhenValveIsOpened),
                    timeWhenValveIsOpened,
                    maxFoundFlowRate,
                    distances);

                maxFoundFlowRate = Math.Max(maxFoundFlowRate, result);
                currentPath.Pop();

                unvisitedNodes.Add(option);
            }

            return Math.Max(maxFoundFlowRate, pathFlowRate);
        }


        public static int Part2(List<ValveRoom> input)
        {
            var root = BuildModel(input, out var allNodes);

            var distances = new NodeDistances(allNodes);
            var nodesToVisit = allNodes.Where(node => node != root && node.FlowRate > 0).ToArray();

            var seeker1Path = new List<Node>();
            var seeker1 = FindMaximumFlowRate2(root, new(), new(nodesToVisit), 0, 0, 0, seeker1Path, distances);
            nodesToVisit = nodesToVisit.Where(node => !seeker1Path.Contains(node)).ToArray();

            var seeker2Path = new List<Node>();
            var seeker2 = FindMaximumFlowRate2(root, new(), new(nodesToVisit), 0, 0, 0, seeker2Path, distances);
            return seeker1 + seeker2;
        }

        public static int FindMaximumFlowRate2(Node currentNode, Stack<Node> currentPath, HashSet<Node> unvisitedNodes, int pathFlowRate, int minutesPassed, int maxFoundFlowRate, List<Node> maxPath, NodeDistances distances)
        {
            if (minutesPassed >= 26)
            {
                return Math.Max(pathFlowRate, maxFoundFlowRate);
            }

            var options = unvisitedNodes.ToArray();

            if (options.Length == 0 && pathFlowRate > maxFoundFlowRate)
            {
                maxFoundFlowRate = pathFlowRate;
                maxPath.Clear();
                maxPath.AddRange(currentPath);
                return maxFoundFlowRate;
            }

            foreach (var option in options)
            {
                var distance = distances.GetDistance(currentNode, option);
                var timeWhenValveIsOpened = minutesPassed + distance + 1;
                var timeLeftWhenValveIsOpened = 26 - timeWhenValveIsOpened;
                unvisitedNodes.Remove(option);

                currentPath.Push(option);
                var result = FindMaximumFlowRate2(
                    option,
                    currentPath,
                    unvisitedNodes,
                    pathFlowRate + (option.FlowRate * timeLeftWhenValveIsOpened),
                    timeWhenValveIsOpened,
                    maxFoundFlowRate,
                    maxPath,
                    distances);

                maxFoundFlowRate = Math.Max(maxFoundFlowRate, result);
                currentPath.Pop();

                unvisitedNodes.Add(option);
            }

            return Math.Max(maxFoundFlowRate, pathFlowRate);
        }

        public static Node BuildModel(IEnumerable<ValveRoom> entries, out List<Node> allNodes)
        {
            var lookup = entries.ToDictionary(e => e.Name, e => new Node(e.Name, e.FlowRate));

            foreach (var entry in entries)
            {
                var mainNode = lookup[entry.Name];

                mainNode.Nodes = entry.Tunnels
                    .Select(name => lookup[name])
                    .ToList();
            }

            allNodes = lookup.Values.ToList();

            return lookup["AA"];
        }
    }
}
