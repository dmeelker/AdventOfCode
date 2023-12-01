using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public class Program
{
    private static readonly List<Tuple<string, int>> _numbers = new()
    {
        Tuple.Create("one", 1),
        Tuple.Create("two", 2),
        Tuple.Create("three", 3),
        Tuple.Create("four", 4),
        Tuple.Create("five", 5),
        Tuple.Create("six", 6),
        Tuple.Create("seven", 7),
        Tuple.Create("eight", 8),
        Tuple.Create("nine", 9),
    };

    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
    }

    public static int Part1(string[] input)
    {
        return input.Select(line => int.Parse($"{ScanForNumberLeft(line, false)}{ScanForNumberRight(line, false)}")).Sum();
    }

    public static int Part2(string[] input)
    {
        return input.Select(line => int.Parse($"{ScanForNumberLeft(line, true)}{ScanForNumberRight(line, true)}")).Sum();
    }

    public static int ScanForNumberLeft(string input, bool checkSpelledNumbers)
    {
        return input.ScanLeft(str =>
        {
            if (str.TryParseDigitAt(0, out var number))
                return number;

            if (checkSpelledNumbers)
                return _numbers.FirstOrDefault(n => str.StartsWith(n.Item1))?.Item2;

            return null;
        }) ?? throw new ArgumentOutOfRangeException();
    }

    public static int ScanForNumberRight(string input, bool checkSpelledNumbers)
    {
        return input.ScanRight(str =>
        {
            if (str.TryParseDigitAt(^1, out var number))
                return number;

            if (checkSpelledNumbers)
                return _numbers.FirstOrDefault(n => str.EndsWith(n.Item1))?.Item2;

            return null;
        }) ?? throw new ArgumentOutOfRangeException();
    }
}
