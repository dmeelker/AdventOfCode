using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Grid<char> ParseInput(string input)
    {
        var chars = input.ToLines().Select(line => line.ToCharArray()).ToArray();

        var grid = new Grid<char>(chars[0].Length, chars.Length);

        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                grid.Set(x, y, chars[y][x]);
            }
        }

        return grid;
    }
}
