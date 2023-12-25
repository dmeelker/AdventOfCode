using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    private record ParsedComponent(string Name, string[] LinkedNames);

    public static Component[] ParseInput(string input)
    {
        var parsedComponents = input.ToLines().Select(ParseComponent).ToArray();
        var allComponentNames = parsedComponents.Select(x => x.Name).Concat(parsedComponents.SelectMany(x => x.LinkedNames)).Distinct();
        var components = allComponentNames.Select(x => new Component(x)).ToDictionary(c => c.Name);

        foreach (var parsedComponent in parsedComponents)
        {
            var component = components[parsedComponent.Name];
            foreach (var linkedName in parsedComponent.LinkedNames)
            {
                component.Connections.Add(components[linkedName]);
                components[linkedName].Connections.Add(component);
            }
        }

        return components.Values.ToArray();
    }

    private static ParsedComponent ParseComponent(string line)
    {
        var parts = line.Split(":");
        return new ParsedComponent(parts[0], parts[1].Split(" ", System.StringSplitOptions.RemoveEmptyEntries));
    }
}
