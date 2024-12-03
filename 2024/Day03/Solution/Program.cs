using Shared;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Solution;

public class Program
{
    private static readonly Regex _mulExpression = new("^mul\\((\\d{1,3}),(\\d{1,3})\\)");

    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 175015740 || part2 != 112272912)
        {
            throw new Exception("Test failed");
        }
    }

    public static long Part1(string[] input)
    {
        var result = 0L;

        foreach (var line in input)
        {
            for (var i = 0; i < line.Length; i++)
            {
                result += ParseMulExpression(line[i..]);
            }
        }

        return result;
    }

    public static long Part2(string[] input)
    {
        var result = 0L;
        var process = true;

        foreach (var line in input)
        {
            for (var i = 0; i < line.Length; i++)
            {
                var subLine = line[i..];

                if (subLine.StartsWith("do()"))
                {
                    process = true;
                }
                else if (subLine.StartsWith("don't()"))
                {
                    process = false;
                }
                else if (process)
                {
                    result += ParseMulExpression(subLine);
                }
            }
        }

        return result;
    }

    private static long ParseMulExpression(string input)
    {
        var match = _mulExpression.Match(input);
        if (match.Success)
        {
            var left = match.Groups[1].Value.ParseInt();
            var right = match.Groups[2].Value.ParseInt();
            return left * right;
        }

        return 0;
    }
}

