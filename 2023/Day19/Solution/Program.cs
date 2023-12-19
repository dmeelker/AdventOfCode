using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Solution;

public record Rule(string Property, char Operator, long Value, string Workflow);
public record Workflow(string Name, List<Rule> Rules, string FallbackWorkflow);
public record Part(Dictionary<string, long> Properties);
public record Input(Part[] Parts, Dictionary<string, Workflow> Workflows);
public record PropertyRange(long From, long To);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 480738 || part2 != 131550418841958L)
        {
            throw new Exception();
        }
    }

    public static long Part1(Input input)
    {
        return input.Parts.Where(part => ProcessPart(part, input))
            .Sum(part => part.Properties.Sum(p => p.Value));
    }

    private static bool ProcessPart(Part part, Input input)
    {
        var workflow = input.Workflows["in"];

        while (true)
        {
            var matchingRule = workflow.Rules.FirstOrDefault(rule => Matches(part, rule));
            var nextWorkflow = matchingRule?.Workflow ?? workflow.FallbackWorkflow;

            if (nextWorkflow == "R")
            {
                return false;
            }
            else if (nextWorkflow == "A")
            {
                return true;
            }

            workflow = input.Workflows[nextWorkflow];
        }

        throw new Exception();
    }

    private static bool Matches(Part part, Rule rule)
    {
        if (part.Properties.TryGetValue(rule.Property, out var value))
        {
            return rule.Operator switch
            {
                '<' => value < rule.Value,
                '>' => value > rule.Value,
                _ => throw new Exception()
            };
        }
        else
        {
            return false;
        }
    }

    public static long Part2(Input input)
    {
        var start = input.Workflows["in"];

        return GetAcceptedRanges(start, input.Workflows)
            .Sum(set =>
                set.Values.Aggregate(1L, (acc, range) => acc * (range.To - range.From + 1))
            );
    }

    private static List<Dictionary<string, PropertyRange>> GetAcceptedRanges(Workflow workflow, Dictionary<string, Workflow> workflows)
    {
        var acceptedRanges = new List<Dictionary<string, PropertyRange>>();

        var ranges = new Dictionary<string, PropertyRange>() {
            {"x",  new PropertyRange(1, 4000) },
            {"m",  new PropertyRange(1, 4000) },
            {"a",  new PropertyRange(1, 4000) },
            {"s",  new PropertyRange(1, 4000) },
        };

        GetAcceptedRanges(workflow, workflows, ranges, acceptedRanges);

        return acceptedRanges;
    }

    private static void GetAcceptedRanges(Workflow workflow, Dictionary<string, Workflow> workflows, Dictionary<string, PropertyRange> currentRanges, List<Dictionary<string, PropertyRange>> acceptedRanges)
    {
        foreach (var rule in workflow.Rules)
        {
            var modifiedRanges = ApplyRule(currentRanges, rule);

            if (rule.Workflow == "A")
            {
                acceptedRanges.Add(new(modifiedRanges));
            }
            else if (rule.Workflow != "R")
            {
                var nextWorkflow = workflows[rule.Workflow];
                GetAcceptedRanges(nextWorkflow, workflows, modifiedRanges, acceptedRanges);
            }

            currentRanges = ApplyRuleNegated(currentRanges, rule);
        }

        if (workflow.FallbackWorkflow == "A")
        {
            acceptedRanges.Add(new(currentRanges));
        }
        else if (workflow.FallbackWorkflow != "R")
        {
            var nextWorkflow = workflows[workflow.FallbackWorkflow];
            GetAcceptedRanges(nextWorkflow, workflows, currentRanges, acceptedRanges);
        }
    }

    private static Dictionary<string, PropertyRange> ApplyRule(Dictionary<string, PropertyRange> currentRanges, Rule rule)
    {
        var modifiedRanges = new Dictionary<string, PropertyRange>(currentRanges);
        var (from, to) = modifiedRanges[rule.Property];

        if (rule.Operator == '<')
        {
            if (to > rule.Value)
            {
                to = rule.Value - 1;
            }
        }
        else if (rule.Operator == '>')
        {
            if (from < rule.Value)
            {
                from = rule.Value + 1;
            }
        }

        modifiedRanges[rule.Property] = new PropertyRange(from, to);

        return modifiedRanges;
    }

    private static Dictionary<string, PropertyRange> ApplyRuleNegated(Dictionary<string, PropertyRange> currentRanges, Rule rule)
    {
        var modifiedRanges = new Dictionary<string, PropertyRange>(currentRanges);
        var (from, to) = modifiedRanges[rule.Property];

        if (rule.Operator == '<')
        {
            if (from < rule.Value)
            {
                from = rule.Value;
            }
        }
        else if (rule.Operator == '>')
        {
            if (to > rule.Value)
            {
                to = rule.Value;
            }
        }

        modifiedRanges[rule.Property] = new PropertyRange(from, to);
        return modifiedRanges;
    }
}
