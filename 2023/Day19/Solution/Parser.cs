using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solution;

public static class Parser
{
    public static Input ParseInput(string input)
    {
        var sections = input.ToSections();

        var workflows = sections[0].ToLines().Select(ParseWorkflow).ToDictionary(f => f.Name);
        var parts = sections[1].ToLines().Select(ParsePart).ToArray();

        return new(parts, workflows);
    }

    private static Workflow ParseWorkflow(string input)
    {
        var name = input.Substring(0, input.IndexOf('{'));
        input = input.StripPrefix(name).StripPrefix("{").StripPostfix("}");
        var ruleStrings = input.Split(',');
        var rules = ruleStrings
            .TakeWhile(str => str.Contains(":"))
            .Select(ParseRule).ToArray();

        var fallbackWorkflow = ruleStrings[^1];

        return new Workflow(name, rules.ToList(), fallbackWorkflow);
    }

    private static Rule ParseRule(string input)
    {
        var match = Regex.Match(input, @"(\w+)([<>])(\d+):(\w+)");

        return new Rule(
            match.Groups[1].Value,
            match.Groups[2].Value[0],
            long.Parse(match.Groups[3].Value),
            match.Groups[4].Value);
    }

    private static Part ParsePart(string input)
    {
        var properties = new Dictionary<string, long>();
        var parts = input.StripPrefix("{").StripPostfix("}").Split(',');

        foreach (var part in parts)
        {
            var propertyParts = part.Split('=');
            properties.Add(propertyParts[0], long.Parse(propertyParts[1]));
        }

        return new Part(properties);
    }
}
