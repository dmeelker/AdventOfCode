using Shared;
using Shared.Dijkstra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

using Group = HashSet<Component>;

public class Component
{
    public string Name { get; }
    public HashSet<Component> Connections { get; } = new();

    public Component(string name)
    {
        Name = name;
    }

    public void BreakConnection(Component other)
    {
        Connections.Remove(other);
        other.Connections.Remove(this);
    }

    public override string ToString()
    {
        return Name;
    }
}

public record Connection(Component Component1, Component Component2)
{
    public int Strength { get; set; } = 0;

    public static Connection CreateNormalized(Component component1, Component component2)
    {
        return component1.Name.CompareTo(component2.Name) < 0 ? new Connection(component1, component2) : new Connection(component2, component1);
    }

    public bool Connects(Component component)
    {
        return Component1 == component || Component2 == component;
    }

    public Component GetConnectingComponent(Component from)
    {
        if (from == Component1)
        {
            return Component2;
        }
        else if (from == Component2)
        {
            return Component1;
        }
        else
        {
            throw new ArgumentException("Component is not part of this connection", nameof(from));
        }
    }

    public override string ToString()
    {
        return $"{Component1.Name} <-> {Component2.Name}";
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
    }

    public static int Part1(Component[] input)
    {
        var components = input.ToDictionary(c => c.Name);
        var connections = input.SelectMany(c => c.Connections.Select(c2 => Connection.CreateNormalized(c, c2))).ToHashSet().ToList();

        TracePaths(input, connections);

        var topConnections = connections.OrderByDescending(c => c.Strength).Take(3).ToList();

        foreach (var connection in topConnections)
        {
            connection.Component1.BreakConnection(connection.Component2);
        }

        var groups = FindGroups(input);
        return groups.Select(g => g.Count).Aggregate((g1, g2) => g1 * g2);
    }

    private static void TracePaths(Component[] input, List<Connection> connections)
    {
        var toCheck = new HashSet<Connection>();

        foreach (var component1 in input)
        {
            foreach (var component2 in input)
            {
                if (component1 == component2)
                    continue;

                toCheck.Add(Connection.CreateNormalized(component1, component2));
            }
        }

        var count = 0;

        foreach (var connection in toCheck)
        {
            TracePath(connection.Component1, connection.Component2, input, connections);
            count++;

            if (count % 1000 == 0)
            {
                Console.WriteLine(count / ((double)toCheck.Count));
            }
        }
    }

    private static void TracePath(Component component1, Component component2, Component[] allComponents, List<Connection> connections)
    {
        var searcher = new DijkstraSearcher2<Component>(
            optionFunction: c => c.Value.Connections,
            costFunction: (c1, c2) => 1);

        var path = searcher.FindPath(component1, component2, allComponents);

        var hitConnections = path!.SlidingWindow(2).Select(window => FindConnection(window[0], window[1]));

        foreach (var hitConnection in hitConnections)
        {
            hitConnection.Strength++;
        }

        Connection FindConnection(Component component1, Component component2)
        {
            return connections.Where(c => c.Connects(component1) && c.Connects(component2)).Single();
        }
    }

    private static List<Group> FindGroups(Component[] components)
    {
        var allComponents = components.ToHashSet();
        var groups = new List<Group>();

        while (allComponents.Count > 0)
        {
            var component = allComponents.First();
            var group = FindGroup(component);
            groups.Add(group);
            allComponents.ExceptWith(group);
        }

        return groups;
    }

    private static Group FindGroup(Component component)
    {
        var group = new Group();

        var open = new Queue<Component>();
        open.Enqueue(component);

        while (open.TryDequeue(out var currentComponent))
        {
            group.Add(currentComponent);
            var nextComponents = currentComponent.Connections.Where(c => !group.Contains(c));

            foreach (var nextComponent in nextComponents)
            {
                open.Enqueue(nextComponent);
            }
        }

        return group;
    }

    public static int Part2(Component[] input)
    {
        return 0;
    }
}
