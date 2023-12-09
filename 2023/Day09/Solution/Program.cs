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

        if (part1 != 1992273652L || part2 != 1012)
        {
            throw new Exception();
        }
    }

    public static long Part1(IEnumerable<long[]> input)
    {
        var values = new List<long>();

        foreach (var history in input)
        {
            var sequences = GenerateSequences(history);
            sequences[^1].Add(0);

            for (var i = sequences.Count - 1; i >= 1; i--)
            {
                sequences[i - 1].Add(sequences[i - 1][^1] + sequences[i][^1]);
            }

            values.Add(sequences[0][^1]);
        }

        return values.Sum();
    }

    public static long Part2(long[][] input)
    {
        return Part1(input.Select(history => history.Reverse().ToArray()));
    }

    private static List<List<long>> GenerateSequences(IEnumerable<long> history)
    {
        var sequences = new List<List<long>>() {
            history.ToList()
        };

        while (!sequences[^1].All(v => v == 0))
        {
            sequences.Add(GenerateSequence(sequences[^1]));
        }

        return sequences;
    }

    private static List<long> GenerateSequence(IEnumerable<long> previous)
    {
        return previous.SlidingWindow(2).Select(window => window[1] - window[0]).ToList();
    }
}
