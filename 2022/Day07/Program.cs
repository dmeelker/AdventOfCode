namespace AoC;

internal class Program
{
    private static void Main(string[] args)
    {
        var input = Parser.Parse(File.ReadAllText("input.txt"));

        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part1: {part1} Part2: {part2}");

        if (part1 != 1490523 || part2 != 12390492)
        {
            throw new Exception();
        }
    }

    private static long Part1(Directory root)
    {
        return root.AllDirectories
            .Where(dir => dir.TotalSize < 100_000)
            .Sum(dir => dir.TotalSize);
    }

    private static long Part2(Directory root)
    {
        var spaceNeeded = 30_000_000 - (70_000_000 - root.TotalSize);

        return root.AllDirectories
            .Where(dir => dir.TotalSize >= spaceNeeded)
            .OrderBy(dir => dir.TotalSize)
            .First().TotalSize;
    }
}