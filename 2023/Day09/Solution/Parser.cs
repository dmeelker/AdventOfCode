using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static long[][] ParseInput(string input)
    {
        return input.ToLines().Select(line => line.Split(' ').Select(long.Parse).ToArray()).ToArray();
    }
}
