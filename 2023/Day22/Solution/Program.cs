using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public record Point(int X, int Y, int Z);
public class Brick
{
    public HashSet<Point> Cubes { get; set; } = new HashSet<Point>();

    public Brick Clone()
    {
        return new()
        {
            Cubes = Cubes.ToHashSet()
        };
    }
}

record SettleReport(int UniqueBricksMoved);

public class Program
{
    static void Main(string[] args)
    {
        var part1 = Part1(Parser.ParseInput(File.ReadAllText("input.txt")));
        var part2 = Part2(Parser.ParseInput(File.ReadAllText("input.txt")));

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 499 || part2 != 95059)
        {
            throw new Exception();
        }
    }

    public static int Part1(Brick[] input)
    {
        var cubeSpace = BuildCubeSpace(input);

        Settle(input, cubeSpace);

        var count = 0;

        foreach (var brick in input)
        {
            if (!CanMove(input.Except(new[] { brick }), cubeSpace.Except(brick.Cubes).ToHashSet()))
            {
                count++;
            }
        }

        return count;
    }

    private static HashSet<Point> BuildCubeSpace(Brick[] input)
    {
        var cubeSpace = new HashSet<Point>();

        foreach (var cube in input.SelectMany(brick => brick.Cubes))
        {
            cubeSpace.Add(cube);
        }

        return cubeSpace;
    }

    private static SettleReport Settle(Brick[] bricks, HashSet<Point> cubes)
    {
        var movedBricks = new HashSet<Brick>();
        var movedAny = false;

        do
        {
            movedAny = false;

            foreach (var brick in bricks)
            {
                if (CanMove(brick, cubes))
                {
                    MoveDown(brick, cubes);
                    movedAny = true;
                    movedBricks.Add(brick);
                }
            }
        } while (movedAny);

        return new(movedBricks.Count);
    }

    private static void MoveDown(Brick brick, HashSet<Point> cubes)
    {
        foreach (var cube in brick.Cubes)
        {
            cubes.Remove(cube);
        }

        brick.Cubes = brick.Cubes.Select(cube => new Point(cube.X, cube.Y, cube.Z - 1)).ToHashSet();

        foreach (var cube in brick.Cubes)
        {
            cubes.Add(cube);
        }
    }

    private static bool CanMove(IEnumerable<Brick> brick, HashSet<Point> cubes)
    {
        return brick.Any(brick => CanMove(brick, cubes));
    }

    private static bool CanMove(Brick brick, HashSet<Point> cubes)
    {
        foreach (var cube in brick.Cubes)
        {
            var locationBelow = new Point(cube.X, cube.Y, cube.Z - 1);

            if (locationBelow.Z < 1)
            {
                return false;
            }

            if (cubes.Contains(locationBelow) && !brick.Cubes.Contains(locationBelow))
            {
                return false;
            }
        }

        return true;
    }

    public static int Part2(Brick[] input)
    {
        Settle(input, BuildCubeSpace(input));

        var count = 0;

        foreach (var brick in input)
        {
            var clonedBricks = input.Except(new[] { brick }).Select(b => b.Clone()).ToArray();

            var report = Settle(clonedBricks, BuildCubeSpace(clonedBricks));
            count += report.UniqueBricksMoved;
        }

        return count;
    }
}