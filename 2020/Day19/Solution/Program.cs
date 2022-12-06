using System;
using System.Collections.Generic;
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
            return input.Values.Count(line => MatchesRule(line, input.Rules[0], input.Rules));
        }

        public static bool MatchesRule(string input, Rule rule, Dictionary<int, Rule> allRules)
        {
            return MatchesRule(input.ToCharArray(), 0, rule, allRules, out var consumed) && consumed == input.Length;
        }

        public static bool MatchesRule(char[] input, int position, Rule rule, Dictionary<int, Rule> allRules, out int consumed)
        {
            if(rule is ConcreteRule){
                var concreteRule = (ConcreteRule)rule;
                consumed = 1;
                return input[position] == concreteRule.Value;
            }
            else if(rule is CompositeRule)
            {
                var compositeRule = (CompositeRule)rule;

                foreach(var group in compositeRule.RuleNumbers)
                {
                    var currentPosition = position;
                    var match = true;
                    foreach(var subRule in group)
                    {
                        if (currentPosition < input.Length && MatchesRule(input, currentPosition, allRules[subRule], allRules, out var subConsumed))
                        {
                            currentPosition += subConsumed;
                        }
                        else
                        { 
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        consumed = currentPosition - position;
                        return true;
                    }
                }
                consumed = 0;
                return false;
            }
            else
            {
                throw new Exception();
            }
        }

        public static int Part2(Input input)
        {
            AddLoops(input);

            return input.Values.Count(line => MatchesRule(line, input.Rules[0], input.Rules));
        }

        public static void AddLoops(Input input)
        {
            input.Rules[8] = new CompositeRule
            {
                Number = 8,
                RuleNumbers = new[] {
                    new [] { 42 },
                    new [] { 42, 8 }
                }
            };

            input.Rules[11] = new CompositeRule
            {
                Number = 11,
                RuleNumbers = new[] {
                    new [] { 42, 31 },
                    new [] { 42, 11, 31 }
                }
            };
        }
    }

    public class Input
    {
        public Dictionary<int, Rule> Rules { get; set; }
        public string[] Values { get; set; }
    }
}

// 177
// 343