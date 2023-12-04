using Shared;
using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static Grid<char> ParseInput(string input)
        {
            var chars = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToCharArray())
                .ToArray();

            var width = chars[0].Length;
            var height = chars.Length;
            var array = new char[height, width];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    array[y, x] = chars[y][x];
                }
            }

            return new Grid<char>(array, '.');
        }
    }
}
