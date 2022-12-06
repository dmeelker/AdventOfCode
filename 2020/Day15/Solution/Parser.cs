using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static long[] ParseInput(string input)
        {
            return input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => long.Parse(line)).ToArray();
        }
    }
}
