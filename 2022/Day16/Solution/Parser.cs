using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solution
{
    public record ValveRoom(string Name, int FlowRate, List<string> Tunnels);

    public static class Parser
    {
        public static List<ValveRoom> ParseInput(string input)
        {
            return input.ToLines().Select(ParseEntry).ToList();
        }

        private static ValveRoom ParseEntry(string line)
        {
            var match = Regex.Match(line, "Valve ([A-Z]*) has flow rate=(\\d*); tunnels? leads? to valves? (.*)");

            return new ValveRoom(
                match.Groups[1].Value,
                match.Groups[2].Value.ParseInt(),
                match.Groups[3].Value.Split(", ").Select(str => str.Trim()).ToList());
        }
    }
}
