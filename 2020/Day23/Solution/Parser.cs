using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static int[] ParseInput(string input)
        {
            return input.ToCharArray().Select(v => int.Parse("" + v)).ToArray();
        }
    }
}
