using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = ParseInput(File.ReadAllText("input.txt"));
            var tree = CreateTree(input);
            
            var rules = tree.Where(rule => CanContainColor(rule, "shiny gold", out var path)).ToArray(); // 316

            var root = tree.Single(rule => rule.Color == "shiny gold");
            var count = CountBags(root); // 11310



            Console.WriteLine("Hello World!");
        }

        public static int CountBags(Rule rule)
        {
            return rule.MustContain.Count + rule.MustContain.Select(CountBags).Sum();
        }
            
        public static bool CanContainColor(Rule rule, string color, out Stack<Rule> path)
        {
            path = new Stack<Rule>();
            return CanContainColor(rule, color, path);
        }

        public static bool CanContainColor(Rule rule, string color, Stack<Rule> path)
        {
            if (path.Contains(rule))
                return false;

            if (rule.MustContain.Any(subRule => subRule.Color == color))
            {
                return true;
            } 
            else
            {
                path.Push(rule);
                return rule.MustContain.Any(subColor => CanContainColor(subColor, color, path));
            }
        }


        public static Rule[] CreateTree(ParsedRule[] rules)
        {
            var lookup = rules.Select(rule => new Rule
            {
                Color = rule.Color
            }).ToDictionary(rule => rule.Color);

            foreach(var rule in rules)
            {
                SetChildren(rule, lookup);
            }

            return lookup.Values.ToArray();
        }

        public static void SetChildren(ParsedRule rule, Dictionary<string, Rule> rules)
        {
            var treeRule = rules[rule.Color];
            treeRule.MustContain = rule.MustContain.Select(subRule => rules[subRule]).ToList();
        }

        public static ParsedRule[] ParseInput(string input)
        {
            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseLine).ToArray();
        }

        public static ParsedRule ParseLine(string line)
        {
            line = line.Substring(0, line.Length - 1) // Remove trailing period
                .Replace(" bags", "")
                .Replace(" bag", "")
                .Trim();

            var parts = line.Split(" contain ");
            var mainColor = parts[0].ToLower();
            var colors = ParseColors(parts[1]);

            return new ParsedRule {
                Color = mainColor,
                MustContain = colors.ToArray()
            };
        }

        public static IEnumerable<string> ParseColors(string input)
        {
            if (input == "no other")
            {
                yield break;
            }

            var colorsParts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach(var colorPart in colorsParts)
            {
                var parts = colorPart.Trim().Split(' ', 2);
                var quantity = int.Parse(parts[0]);
                var color = parts[1].ToLower();

                for (var i = 0; i < quantity; i++) {
                    yield return color;
                }
            }
        }
    }

    class ParsedRule
    {
        public string Color { get; set; }
        public string[] MustContain { get; set; }
    }

    class Rule
    {
        public string Color { get; set; }
        public List<Rule> MustContain { get; set; }
    }
}
