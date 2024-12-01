using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static int[][] ParseInput(string input)
    {
        var pairs = input
            .ToLines()
            .Select(line => line.ParseIntArray(" "))
            .ToArray();

        return [
            pairs.Select(values => values[0]).ToArray(),
            pairs.Select(values => values[1]).ToArray()
        ];
    }
}
