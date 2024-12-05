using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public record Rule(int Page1, int Page2)
{
    public bool AppliesTo(int page1, int page2)
    {
        return
            (Page1 == page1 || Page2 == page1) &&
            (Page1 == page2 || Page2 == page2);
    }
}

public record Input(List<Rule> Rules, List<List<int>> Updates);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 6260 || part2 != 5346)
        {
            throw new Exception("Test failed");
        }
    }

    public static int Part1(Input input)
    {
        return input.Updates
            .Where(u => CheckValid(u, input.Rules))
            .Sum(GetMiddleNumber);
    }

    public static int Part2(Input input)
    {
        return input.Updates
            .Where(update => !CheckValid(update, input.Rules))
            .Select(update => update.OrderBy(u => u, new UpdateComparer(input.Rules)).ToList())
            .Sum(GetMiddleNumber);
    }

    private static bool CheckValid(List<int> update, List<Rule> rules)
    {
        foreach (var rule in rules)
        {
            if (update.Contains(rule.Page1) && update.Contains(rule.Page2))
            {
                var page1Index = update.IndexOf(rule.Page1);
                var page2Index = update.IndexOf(rule.Page2);
                if (page1Index > page2Index)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static int GetMiddleNumber(List<int> update) => update[(int)Math.Floor(update.Count / 2.0)];
}

public class UpdateComparer(List<Rule> rules) : IComparer<int>
{
    public int Compare(int page1, int page2)
    {
        var rule = rules.FirstOrDefault(r => r.AppliesTo(page1, page2));

        if (rule == null)
        {
            return 0;
        }
        else if (page1 == rule.Page2)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}

