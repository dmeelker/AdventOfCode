
namespace AoC;

public record Move(string Direction, int Distance);

internal static class Parser
{
    public static Move[] Parse(string[] input)
    {
        return input.Select(line =>
        {
            var parts = line.Split(" ");
            return new Move(parts[0], int.Parse(parts[1]));
        }).ToArray();
    }
}
