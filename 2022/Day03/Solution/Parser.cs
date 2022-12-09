using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static List<IEnumerable<char>> ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray().AsEnumerable()).ToList();
        }
    }
}
