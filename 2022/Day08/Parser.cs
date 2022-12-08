
namespace AoC;

internal static class Parser
{
    public static int[,] Parse(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;

        var grid = new int[width, height];

        for (var y = 0; y < input.Length; y++)
        {
            var cells = input[y].ToCharArray();
            for (var x = 0; x < width; x++)
            {
                grid[x, y] = int.Parse(cells[x].ToString());
            }
        }
        return grid;
    }
}
