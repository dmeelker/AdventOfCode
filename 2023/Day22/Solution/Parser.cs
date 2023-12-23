using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Brick[] ParseInput(string input)
    {
        return input.ToLines().Select(ParseBrick).ToArray();
    }

    private static Brick ParseBrick(string line)
    {
        var parts = line.Split('~');
        var start = ParseCoordinate(parts[0]);
        var end = ParseCoordinate(parts[1]);
        var cubes = new List<Point>();

        for (var x = start.X; x <= end.X; x++)
        {
            for (var y = start.Y; y <= end.Y; y++)
            {
                for (var z = start.Z; z <= end.Z; z++)
                {
                    cubes.Add(new Point(x, y, z));
                }
            }
        }

        return new Brick
        {
            Cubes = cubes.ToHashSet()
        };
    }

    private static Point ParseCoordinate(string input)
    {
        var parts = input.Split(',').Select(int.Parse).ToArray();

        return new(parts[0], parts[1], parts[2]);
    }
}
