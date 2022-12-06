using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(Input input)
        {
            return input.OtherTickets.SelectMany(ticket => ticket)
                .Where(value => !input.Rules.Any(rule => rule.Matches(value)))
                .Sum();
        }

        public static long Part2(Input input)
        {
            var validTickets = input.OtherTickets.Where(ticket => IsValidTicket(input.Rules, ticket)).ToArray();
            var mapping = FindMapping(input.Rules, validTickets).ToDictionary(m => m.rule, m => m.column);

            return input.Rules.Where(rule => rule.Name.ToLower().StartsWith("departure"))
                .Select(rule => (long)input.YourTicket[mapping[rule]])
                .Aggregate((x, y) => x * y);
        }

        public static bool IsValidTicket(Rule[] rules, int[] ticket) => ticket.All(value => rules.Any(rule => rule.Matches(value)));

        private static IEnumerable<(Rule rule, int column)> FindMapping(Rule[] rules, int[][] tickets)
        {
            var columns = GetMatchingRules(rules, tickets)
                .OrderBy(column => column.rules.Length)
                .ToArray();

            while(columns.Length > 0)
            {
                Debug.Assert(columns.First().rules.Length == 1);
                var lastColumn = columns.First();
                yield return (lastColumn.rules[0], lastColumn.column);

                columns = columns.Select(column => (column: column.column, rules: column.rules.Where(r => r != lastColumn.rules[0]).ToArray()))
                    .Where(column => column.column != lastColumn.column)
                    .OrderBy(column => column.rules.Length)
                    .ToArray();
            }
        }

        public static IEnumerable<(int column, Rule[] rules)> GetMatchingRules(Rule[] rules, int[][] tickets)
        {
            return Enumerable.Range(0, tickets[0].Length)
                .Select(column => (index: column, values: tickets.Select(ticket => ticket[column]).ToArray()))
                .Select(column => (index: column.index, rules: rules.Where(rule => column.values.All(value => rule.Matches(value))).ToArray()));
        }
    }
}