using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static InputEntry[] ParseInput(string input)
    {
        return input.ToLines().Select(ParseInstruction).ToArray();
    }

    private static InputEntry ParseInstruction(string input)
    {
        var parts = input.Replace("(", "")
            .Replace(")", "")
            .Split(' ');

        return new InputEntry(parts[0][0], int.Parse(parts[1]), parts[2]);
    }
}
