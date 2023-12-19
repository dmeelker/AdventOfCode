using Shared;

namespace Solution;

public static class Parser
{
    public static Grid<int> ParseInput(string input)
    {
        return Grid<int>.ParseIntGrid(input.ToLines());
    }
}
