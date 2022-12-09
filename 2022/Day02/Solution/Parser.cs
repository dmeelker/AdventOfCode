using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static IEnumerable<Tuple<char, char>> ParseInput(string input)
        {
            var lines = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(parts => Tuple.Create(parts[0][0], parts[1][0]));
        }
    }
}
