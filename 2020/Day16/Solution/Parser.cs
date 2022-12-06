using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static Input ParseInput(string input)
        {
            var lines = new Queue<string>(input.Split('\n').Select(line => line.Trim()));

            var result = new Input();
            result.Rules = ReadRules(lines).ToArray();

            lines.Dequeue();
            result.YourTicket = ParseTicket(lines.Dequeue());
            lines.Dequeue();

            lines.Dequeue();
            result.OtherTickets = ParseTickets(lines).ToArray();

            return result;
        }

        private static IEnumerable<Rule> ReadRules(Queue<string> lines)
        {
            while (lines.Peek() != "")
            {
                yield return ParseRule(lines.Dequeue());
            }

            lines.Dequeue();
        }

        private static Rule ParseRule(string line)
        {
            var mainParts = line.Split(':');
            var rangeParts = mainParts[1].Split(" or ");

            return new Rule { 
                Name = mainParts[0],
                Ranges = rangeParts.Select(ParseRange).ToArray()
            };
        }

        private static Range ParseRange(string input)
        {
            var parts = input.Split('-');

            return new Range(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        private static IEnumerable<int[]> ParseTickets(Queue<string> lines)
        {
            while (lines.Count > 0)
            {
                yield return ParseTicket(lines.Dequeue());
            }
        }

        private static int[] ParseTicket(string line)
        {
            return line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(value => int.Parse(value)).ToArray();
        }
    }

    public class Input
    {
        public Rule[] Rules { get; set; }
        public int[] YourTicket { get; set; }
        public int[][] OtherTickets { get; set; }
    }

    public class Rule
    {
        public string Name { get; set; }
        public Range[] Ranges { get; set; }

        public bool Matches(int input) => Ranges.Any(range => range.Contains(input));

        public override string ToString()
        {
            return Name;
        }
    }

    public class Range
    {
        public int From { get; }
        public int To { get; }

        public Range(int from, int to)
        {
            From = from;
            To = to;
        }

        public bool Contains(int value) => value >= From && value <= To;
    }
}
