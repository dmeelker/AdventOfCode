using Shared;
using System;
using System.Collections.Generic;
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

        if (part1 != 379 || part2 != 430)
        {
            throw new Exception("Test failed");
        }
    }

    public static int Part1(int[][] input)
    {
        return input.Where(IsSafe).Count();
    }

    private static bool IsSafe(IEnumerable<int> report)
    {
        var differences = report.SlidingWindow(2).Select(pair => pair[1] - pair[0]);

        return
            differences.All(d => d != 0) &&
            differences.Select(Math.Sign).CountDistinct() == 1 &&
            differences.All(diff => Math.Abs(diff) <= 3);
    }

    public static int Part2(int[][] input)
    {
        return input.Where(report =>
        {
            if (IsSafe(report))
            {
                return true;
            }

            for (int i = 0; i < report.Length; i++)
            {
                var newReport = report.ToList();
                newReport.RemoveAt(i);

                if (IsSafe(newReport))
                {
                    return true;
                }
            }

            return false;
        }).Count();
    }
}

