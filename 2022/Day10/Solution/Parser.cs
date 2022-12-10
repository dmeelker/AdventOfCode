using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public record Operation(string Name, int? Parameter);
    public static class Parser
    {
        public static IEnumerable<Operation> ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line =>
                {
                    var parts = line.Split(" ");

                    if (parts.Length == 1)
                    {
                        return new Operation(parts[0], null);
                    }
                    else
                    {
                        return new Operation(parts[0], int.Parse(parts[1]));
                    }
                });
        }
    }
}
