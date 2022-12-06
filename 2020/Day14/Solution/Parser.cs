using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solution
{
    public static class Parser
    {
        public static Block[] ParseInput(string input)
        {
            var lines = new Queue<string[]>(
                input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split('=').Select(part => part.Trim()).ToArray()));

            return ReadBlocks(lines).ToArray();
        }

        public static IEnumerable<Block> ReadBlocks(Queue<string[]> lines)
        {
            while (lines.Count > 0)
            {
                yield return new Block()
                {
                    Mask = lines.Dequeue()[1],
                    Operations = ReadOperations(lines).ToArray(),
                };
            }
        }

        public static IEnumerable<Operation> ReadOperations(Queue<string[]> input)
        {
            while (input.Count > 0 && input.Peek()[0] != "mask")
            {
                var line = input.Dequeue();
                yield return new Operation(
                    ParseMemoryAddress(line[0]),
                    long.Parse(line[1])
                );
            }
        }

        private static int ParseMemoryAddress(string input)
        {
            var match = Regex.Match(input, @"\d+");
            return int.Parse(match.Groups[0].Value);
        }
    }
}
