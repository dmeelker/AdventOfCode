using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static List<int> ParseInput(string input)
        {
            return input.ToLines().Select(line => line.ParseInt()).ToList();
        }
    }
}
