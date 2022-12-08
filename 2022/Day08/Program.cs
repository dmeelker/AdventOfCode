namespace AoC;


internal class Program
{
    public static readonly Vector Left = new(-1, 0);
    public static readonly Vector Right = new(1, 0);
    public static readonly Vector Top = new(0, -1);
    public static readonly Vector Bottom = new(0, 1);

    private static void Main(string[] args)
    {
        var input = Parser.Parse(File.ReadAllLines("input.txt"));

        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part1: {part1} Part2: {part2}");

        if (part1 != 1533 || part2 != 345744)
        {
            throw new Exception();
        }
    }

    private static long Part1(int[,] input)
    {
        return EachCell(input)
            .Count(location =>
                TreesInAllDirections(input, location)
                    .Any(path => path.All(height => height < input[location.X, location.Y]))
            );
    }
    private static long Part2(int[,] input)
    {
        return EachCell(input)
            .Max(location =>
                TreesInAllDirections(input, location)
                    .Select(path =>
                        path.Select((height, index) => new { Height = height, Score = index + 1 })
                            .FirstOrDefault(path => path.Height >= input[location.X, location.Y])?.Score ?? path.Count()
                    ).Aggregate((x, y) => x * y)
            );
    }

    private static IEnumerable<Vector> EachCell(int[,] input)
    {
        for (var x = 0; x < input.GetLength(0); x++)
        {
            for (var y = 0; y < input.GetLength(1); y++)
            {
                yield return new(x, y);
            }
        }
    }

    private static IEnumerable<IEnumerable<int>> TreesInAllDirections(int[,] input, Vector start)
    {
        yield return TreesInDirection(input, start, Left);
        yield return TreesInDirection(input, start, Right);
        yield return TreesInDirection(input, start, Top);
        yield return TreesInDirection(input, start, Bottom);
    }

    private static IEnumerable<int> TreesInDirection(int[,] input, Vector start, Vector direction)
    {
        var location = start.Add(direction);

        while (LocationInInput(input, location))
        {
            yield return input[location.X, location.Y];
            location = location.Add(direction);
        }
    }

    private static bool LocationInInput(int[,] input, Vector location)
    {
        return location.X >= 0 &&
            location.Y >= 0 &&
            location.X < input.GetLength(0) &&
            location.Y < input.GetLength(1);
    }
}