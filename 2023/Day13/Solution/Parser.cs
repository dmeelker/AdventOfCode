using Shared;
using System.Linq;

namespace Solution;

public static class Parser
{
    public static Grid<char>[] ParseInput(string input)
    {
        return input.ToSections().Select(section => Grid<char>.ParseCharGrid(section.ToLines())).ToArray();
    }
}
