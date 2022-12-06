using AoC;

internal class Program
{
    private static void Main(string[] args)
    {
        var input = new Parser().ReadInput(File.ReadAllLines("input.txt"));

        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part1: {part1} Part2: {part2}");
    }

    private static object Part1(IEnumerable<Tuple<Range, Range>> input)
    {
        return input.Count(pair => Contains(pair.Item1, pair.Item2) || Contains(pair.Item2, pair.Item1));
    }

    private static object Part2(IEnumerable<Tuple<Range, Range>> input)
    {
        return input.Count(pair => Overlaps(pair.Item1, pair.Item2));
    }

    private static bool Contains(Range range1, Range range2)
    {
        return range2.Start.Value >= range1.Start.Value && range2.End.Value <= range1.End.Value;
    }

    private static bool Overlaps(Range range1, Range range2)
    {
        return range2.Start.Value <= range1.End.Value && range2.End.Value >= range1.Start.Value;
    }
}