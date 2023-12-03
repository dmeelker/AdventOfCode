using AoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static List<List<Vector>> ParseInput(string input)
        {
            var lines = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(ParseLine).ToList();
        }

        private static List<Vector> ParseLine(string line)
        {
            return line.Split("), (")
                .Select(part =>
                    ParseVector(part.Trim('(', ')')))
                .ToList();
        }

        private static Vector ParseVector(string input)
        {
            var parts = input.Split(',')
                .Select(int.Parse)
                .ToArray();

            return new Vector(parts[0], parts[1]);
        }
    }
}
