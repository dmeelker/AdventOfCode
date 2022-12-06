internal class Program
{
    private static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");

        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part1: {part1} Part2: {part2}");
    }

    private static int Part1(string input)
    {
        return FindFirstDistinctBlock(input, 4);
    }

    private static int Part2(string input)
    {
        return FindFirstDistinctBlock(input, 14);
    }

    private static int FindFirstDistinctBlock(string input, int length)
    {
        for (var i = 0; i < input.Length - length; i++)
        {
            var word = input.Substring(i, length);
            if (word.ToCharArray().Distinct().Count() == length)
            {
                return i + length;
            }
        }

        return 0;
    }
}