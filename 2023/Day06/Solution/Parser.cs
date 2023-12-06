using Shared;
using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static int[][] ParseInput(string input)
        {
            return input.ToLines()
                .Select(line => line.Substring(line.IndexOf(':') + 1))
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .ToArray();
        }
    }
}
