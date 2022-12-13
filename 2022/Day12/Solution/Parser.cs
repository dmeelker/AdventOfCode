using Shared;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static Grid<char> ParseInput(string input)
        {
            var cells = input.ToLines().Select(line => line.ToCharArray()).ToArray();

            var grid = new Grid<char>(cells[0].Length, cells.Length, '.');

            for (var x = 0; x < grid.Width; x++)
            {
                for (var y = 0; y < grid.Height; y++)
                {
                    grid.Set(new(x, y), cells[y][x]);
                }
            }

            return grid;
        }
    }
}
