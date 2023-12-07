using System;
using System.IO;

namespace Solution;

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("testinput.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
    }

    public static int Part1(string[] input)
    {
        return 0;
    }

    public static int Part2(string[] input)
    {
        return 0;
    }
}
