using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static List<Point> ParseInput(string input)
        {
            return input.ToLines().SelectMany(ParseLine).ToList();
        }

        private static IEnumerable<Point> ParseLine(string line, int y)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '#')
                {
                    yield return new(i, y);
                }
            }
        }
    }
}
