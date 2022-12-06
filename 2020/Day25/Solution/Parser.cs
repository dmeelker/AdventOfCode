using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static int[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => int.Parse(line))
                .ToArray();
        }
    }
}
