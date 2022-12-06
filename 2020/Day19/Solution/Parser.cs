using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static Input ParseInput(string input)
        {
            var sections = input.Replace("\r", "").Split("\n\n");

            return new Input
            {
                Rules = ParseRules(sections[0].Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToArray()),
                Values = sections[1].Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToArray()
            };
        }

        private static Dictionary<int, Rule> ParseRules(string[] lines)
        {
            var result = new Dictionary<int, Rule>();

            foreach(var line in lines)
            {
                var mainparts = line.Split(": ");
                var number = int.Parse(mainparts[0]);

                if(mainparts[1].StartsWith("\""))
                {
                    result.Add(number, new ConcreteRule {
                        Number = number,
                        Value = mainparts[1][1]
                    });
                } else
                {
                    var groups = mainparts[1].Split('|');

                    result.Add(number, new CompositeRule
                    {
                        Number = number,
                        RuleNumbers = groups.Select(group => group.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id)).ToArray())
                        .OrderByDescending(group => group.Any(id => id == number))
                        .ToArray()
                    });
                }
            }

            return result;
        }
    }

    public class Rule
    {
        public int Number { get; set; }
    }

    public class ConcreteRule : Rule
    {
        public char Value { get; set; }
    }

    public class CompositeRule : Rule
    {
        public int[][] RuleNumbers { get; set; }

        public bool IsGroupRecursive(int index)
        {
            return RuleNumbers[index].Any(id => id == Number);
        }
    }
}
