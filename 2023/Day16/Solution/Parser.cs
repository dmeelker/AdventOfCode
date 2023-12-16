using Shared;

namespace Solution;

public static class Parser
{
    public static Grid<char> ParseInput(string input)
    {
        return Grid<char>.ParseCharGrid(input.ToLines());
    }
}
