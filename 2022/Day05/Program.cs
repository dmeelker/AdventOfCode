using AoC;

internal class Program
{
    private static void Main(string[] args)
    {
        var part1 = Part1(ReadInput());
        var part2 = Part2(ReadInput());

        Console.WriteLine($"Part1: {part1} Part2: {part2}");
    }

    private static PuzzleInput ReadInput()
    {
        return new Parser().ReadInput(File.ReadAllText("testinput.txt"));
    }

    private static string Part1(PuzzleInput input)
    {
        foreach (var operation in input.Operations)
        {
            Enumerable.Range(0, operation.Quantity)
                .Select(_ => input.Stacks[operation.From - 1].Pop())
                .ToList()
                .ForEach(value => input.Stacks[operation.To - 1].Push(value));
        }

        return GetStackTopsAsString(input.Stacks);
    }

    private static string Part2(PuzzleInput input)
    {
        foreach (var operation in input.Operations)
        {
            Enumerable.Range(0, operation.Quantity)
                .Select(_ => input.Stacks[operation.From - 1].Pop())
                .Reverse()
                .ToList()
                .ForEach(value => input.Stacks[operation.To - 1].Push(value));
        }

        return GetStackTopsAsString(input.Stacks);
    }

    private static string GetStackTopsAsString(Stack<string>[] stacks)
    {
        return string.Concat(stacks.Select(stack => stack.Pop()));
    }
}