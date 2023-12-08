using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Input ParseInput(string input)
    {
        var sections = input.ToSections();
        var directions = sections[0];

        var nodes = sections[1]
            .ToLines()
            .Select(ParseNode)
            .ToArray();

        return new Input(directions, nodes);
    }

    private static Node ParseNode(string input)
    {
        var parts = input.Replace("(", "")
            .Replace(")", "")
            .Replace("=", " ")
            .Replace(",", " ")
            .Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        return new Node(parts[0], parts[1], parts[2]);
    }
}
