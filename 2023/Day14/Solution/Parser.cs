using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static MirrorField ParseInput(string input)
    {
        var chars = input.ToLines().Select(line => line.ToCharArray()).ToArray();

        var grid = new char[chars[0].Length, chars.Length];

        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = chars[y][x];
            }
        }


        return new MirrorField(grid); ;
    }
}