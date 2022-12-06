using System;
using System.Collections.Generic;
using System.Text;

namespace Solution
{
    public static class InputExtensions
    {
        public static char[,] Transform(this char[,] input, Func<(int x, int y), char> transformation)
        {
            var result = new char[input.GetLength(0), input.GetLength(1)];

            for (var x = 0; x < input.GetLength(0); x++)
                for (var y = 0; y < input.GetLength(1); y++)
                    result[x, y] = transformation((x, y));

            return result;
        }

        public static bool ContainsPoint(this char[,] input, (int x, int y) point)
        {
            return point.x >= 0 && point.y >= 0 && point.x < input.GetLength(0) && point.y < input.GetLength(1);
        }

        public static bool IsEqual(this char[,] state1, char[,] state2)
        {
            for (var x = 0; x < state1.GetLength(0); x++)
            {
                for (var y = 0; y < state1.GetLength(1); y++)
                {
                    if (state1[x, y] != state2[x, y])
                        return false;
                }
            }

            return true;
        }

        public static int CountCells(this char[,] input, Func<char, bool> filter)
        {
            int count = 0;

            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    if (filter(input[x, y]))
                        count++;
                }
            }

            return count;
        }

        public static string AsString(this char[,] input)
        {
            var str = new StringBuilder();
            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    str.Append(input[x, y]);
                }
                str.AppendLine();
            }

            return str.ToString();
        }
    }
}
