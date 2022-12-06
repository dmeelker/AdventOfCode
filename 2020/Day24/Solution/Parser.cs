using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solution
{
    public static class Parser
    {
        public static Point[][] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => ParseLine(line).ToArray())
                .ToArray();
        }

        public static IEnumerable<Point> ParseLine(string line)
        {
            var i = 0;

            while (i < line.Length)
            {
                var instruction = Regex.Match(line.Substring(i, i < line.Length - 1 ? 2 : 1), "^e|w|se|sw|ne|nw$", RegexOptions.Singleline).Groups[0].Value;

                yield return instruction switch { 
                    "e" => Directions.East,
                    "w" => Directions.West,
                    "se" => Directions.SouthEast,
                    "sw" => Directions.SouthWest,
                    "nw" => Directions.NorthWest,
                    "ne" => Directions.NorthEast,
                    _ => throw new Exception()
                };
                i += instruction.Length;
            }
        }
    }
}
