using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Hand[] ParseInput(string input)
    {
        return input.ToLines()
            .Select(line =>
            {
                var parts = line.Split(' ');
                return new Hand(parts[0].ToCharArray(), int.Parse(parts[1]));
            }).ToArray();
    }
}
