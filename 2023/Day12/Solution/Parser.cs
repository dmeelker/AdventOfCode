using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Row[] ParseInput(string input)
    {
        return input.ToLines().Select(ParseLine).ToArray();
    }

    private static Row ParseLine(string line)
    {
        var parts = line.Split(' ');
        var sequences = parts[1].Split(',').Select(int.Parse).ToArray();

        return new(parts[0], sequences);
    }
}
