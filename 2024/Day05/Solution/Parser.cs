using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Input ParseInput(string input)
    {
        var sections = input.ToSections();

        return new(
            Rules: sections[0].ToLines().Select(line =>
                {
                    var parts = line.ParseIntArray("|");
                    return new Rule(parts[0], parts[1]);
                }).ToList(),
            Updates: sections[1].ToLines().Select(line => line.ParseIntArray(",").ToList()).ToList()
        );
    }
}
