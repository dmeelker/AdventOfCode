using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static int[][] ParseInput(string input)
        {
            input = input.Replace("\r", "");
            return input.Split("\n\n").Select(
                    section => section.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .Select(line => int.Parse(line))
                    .ToArray())
                .ToArray();
        }
    }
}
