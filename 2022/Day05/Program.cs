using AoC;

internal class Program
{
    private static void Main(string[] args)
    {
        var input = new Parser().ReadInput(File.ReadAllText("testinput.txt"));

        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part1: {part1} Part2: {part2}");
    }

    private static Stack<string>[] PrepareStacks(string[][] input)
    {
        return input.Select(stack => new Stack<string>(stack)).ToArray();
    }

    private static string Part1(PuzzleInput input)
    {
        var stacks = PrepareStacks(input.Stacks);
        foreach (var operation in input.Operations)
        {
            Enumerable.Range(0, operation.Quantity)
                .Select(_ => stacks[operation.From - 1].Pop())
                .ToList()
                .ForEach(value => stacks[operation.To - 1].Push(value));
        }

        return string.Join("", stacks.Select(stack => stack.Pop()));
    }

    private static string Part2(PuzzleInput input)
    {
        var stacks = PrepareStacks(input.Stacks);
        foreach (var operation in input.Operations)
        {
            Enumerable.Range(0, operation.Quantity)
                .Select(_ => stacks[operation.From - 1].Pop())
                .Reverse()
                .ToList()
                .ForEach(value => stacks[operation.To - 1].Push(value));
        }

        return string.Join("", stacks.Select(stack => stack.Pop()));
    }
}