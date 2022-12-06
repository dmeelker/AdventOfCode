using System;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static Field ParseInput(string input)
        {
            var data = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToCharArray()).ToArray();

            int width = data[0].Length;
            int height = data.Length;
            var field = new Field(width, height, 1, 1);

            for (var x=0; x<width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    field.Set(new Point(x, 0, y, 0), data[y][x] == '#');
                }
            }

            return field;
        }
    }
}
