using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static int[][] ParseInput(string input)
    {
        return input.ToLines(skipEmpty: true)
            .Select(line => line.ParseIntArray(" "))
            .ToArray();
    }
}
