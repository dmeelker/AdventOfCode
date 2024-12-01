using System;
using System.IO;
using System.Linq;

namespace Solution;

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 3246517 || part2 != 29379307)
        {
            throw new Exception("Wrong answer");
        }
    }

    public static int Part1(int[][] input)
    {
        var p1 = input[0].OrderBy(x => x).ToArray();
        var p2 = input[1].OrderBy(x => x).ToArray();

        return p1
            .Zip(p2)
            .Select(pair => Math.Abs(pair.First - pair.Second)).Sum();
    }

    public static int Part2(int[][] input)
    {
        return input[0]
            .Select(left => left * input[1].Count(right => right == left))
            .Sum();
    }
}
