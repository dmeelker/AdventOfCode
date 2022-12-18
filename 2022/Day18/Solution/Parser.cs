using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static List<Point3> ParseInput(string input)
        {
            return input.ToLines().Select(line =>
            {
                var parts = line.ParseIntArray();
                return new Point3(parts[0], parts[1], parts[2]);
            }).ToList();
        }
    }
}
