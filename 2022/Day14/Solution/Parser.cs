using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public record Path(List<Point> Points);

    public static class Parser
    {
        public static List<Path> ParseInput(string input)
        {
            return input.ToLines().Select(ParsePath).ToList();
        }

        public static Path ParsePath(string input)
        {
            return new(input.Split(" -> ").Select(ParsePoint).ToList());
        }

        public static Point ParsePoint(string input)
        {
            var parts = input.Split(",");
            return new(int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
