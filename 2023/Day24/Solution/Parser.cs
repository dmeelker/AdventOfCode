using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Line3D[] ParseInput(string input)
    {
        return input.ToLines().Select(ParseLine).ToArray();
    }

    private static Line3D ParseLine(string line)
    {
        var parts = line.Split("@");

        return new Line3D(ParseVector(parts[0]), ParseVector(parts[1]));
    }

    private static Vector3D ParseVector(string input)
    {
        var parts = input.Split(',').Select(long.Parse).ToArray();
        return new Vector3D(parts[0], parts[1], parts[2]);
    }
}
