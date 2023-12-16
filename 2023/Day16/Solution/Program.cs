using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

record Beam(Point Location, Point Direction)
{
    public Beam NextLocation() => new Beam(Location.Add(Direction), Direction);
}

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 7788 || part2 != 7987)
        {
            throw new Exception();
        }
    }

    public static int Part1(Grid<char> input)
    {
        return Solve(input, new Beam(new(0, 0), new Point(1, 0)));
    }

    public static int Part2(Grid<char> input)
    {
        return Enumerable.Range(0, input.Width)
            .SelectMany(i => new[] {
                new Beam(new(i, 0), new Point(0, 1)),
                new Beam(new(i, input.Height - 1), new Point(0, -1)),
                new Beam(new(0, i), new Point(1, 0)),
                new Beam(new(input.Width - 1, i), new Point(-1, 0))
            })
            .Max(entrance => Solve(input, entrance));
    }

    private static int Solve(Grid<char> input, Beam start)
    {
        var visited = new HashSet<Beam>();
        var beamStack = new Stack<Beam>();
        beamStack.Push(start);

        while (beamStack.TryPop(out var beam))
        {
            if (!input.Contains(beam.Location) || visited.Contains(beam))
            {
                continue;
            }

            visited.Add(beam);

            beamStack.Push(input.Get(beam.Location) switch
            {
                '.' => new[] { beam.NextLocation() },
                '-' => HorizontalSplitter(beam),
                '|' => VerticalSplitter(beam),
                '/' => MirrorRight(beam),
                '\\' => MirrorLeft(beam),
                _ => throw new Exception()
            });
        }

        return visited.Select(b => b.Location).Distinct().Count();
    }

    private static IEnumerable<Beam> HorizontalSplitter(Beam beam)
    {
        if (beam.Direction.IsHorizontal())
        {
            yield return beam.NextLocation();
        }
        else
        {
            yield return new Beam(beam.Location.Add(new Point(-1, 0)), Point.Left);
            yield return new Beam(beam.Location.Add(new Point(+1, 0)), Point.Right);
        }
    }

    private static IEnumerable<Beam> VerticalSplitter(Beam beam)
    {
        if (beam.Direction.IsVertical())
        {
            yield return beam.NextLocation();
        }
        else
        {
            yield return new Beam(beam.Location.Add(new Point(0, -1)), Point.Up);
            yield return new Beam(beam.Location.Add(new Point(0, +1)), Point.Down);
        }
    }

    private static IEnumerable<Beam> MirrorLeft(Beam beam)
    {
        yield return (beam.Direction.X, beam.Direction.Y) switch
        {
            (0, -1) => new Beam(beam.Location.Add(Point.Left), Point.Left),
            (0, +1) => new Beam(beam.Location.Add(Point.Right), Point.Right),
            (-1, 0) => new Beam(beam.Location.Add(Point.Up), Point.Up),
            (+1, 0) => new Beam(beam.Location.Add(Point.Down), Point.Down),
            _ => throw new Exception()
        };
    }

    private static IEnumerable<Beam> MirrorRight(Beam beam)
    {
        yield return (beam.Direction.X, beam.Direction.Y) switch
        {
            (0, -1) => new Beam(beam.Location.Add(Point.Right), Point.Right),
            (0, +1) => new Beam(beam.Location.Add(Point.Left), Point.Left),
            (-1, 0) => new Beam(beam.Location.Add(Point.Down), Point.Down),
            (+1, 0) => new Beam(beam.Location.Add(Point.Up), Point.Up),
            _ => throw new Exception()
        };
    }
}

public static class PointExtensions
{
    public static bool IsHorizontal(this Point point) => point.X != 0;
    public static bool IsVertical(this Point point) => point.Y != 0;
}
