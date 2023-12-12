using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Solution;

public record Row(string Springs, int[] Sequences);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        if (part1 != 7916L || part2 != 37366887898686L)
        {
            throw new Exception();
        }
    }

    static long Part1(Row[] input) => Solve(input, 1);

    static long Part2(Row[] input) => Solve(input, 5);

    static long Solve(Row[] rows, int repeat)
    {
        return rows
            .Select(row => Unfold(row, repeat))
            .Sum(row => Compute(row.Springs, ImmutableStack.CreateRange(row.Sequences.Reverse()), new()));
    }

    static string CacheKey(string pattern, ImmutableStack<int> sequences)
    {
        return pattern + ":" + string.Join(',', sequences);
    }

    static long Compute(string pattern, ImmutableStack<int> sequences, Dictionary<string, long> cache)
    {
        var key = CacheKey(pattern, sequences);
        if (!cache.ContainsKey(key))
        {
            cache[key] = MatchCharacter(pattern, sequences, cache);
        }

        return cache[key];
    }

    static long MatchCharacter(string springs, ImmutableStack<int> sequences, Dictionary<string, long> cache)
    {
        if (springs.Length == 0)
        {
            return sequences.Any() ? 0 : 1;
        }

        return springs.First() switch
        {
            '.' => Compute(springs[1..], sequences, cache),
            '?' => Compute('.' + springs[1..], sequences, cache) + Compute('#' + springs[1..], sequences, cache),
            '#' => ProcessHash(springs, sequences, cache),
            _ => throw new Exception()
        };
    }

    private static long ProcessHash(string springs, ImmutableStack<int> sequences, Dictionary<string, long> cache)
    {
        if (!sequences.Any())
        {
            return 0;
        }

        var expectedSprings = sequences.Peek();
        sequences = sequences.Pop();

        var potentiallyDead = springs.TakeWhile(s => s == '#' || s == '?').Count();

        if (potentiallyDead < expectedSprings)
        {
            return 0;
        }
        else if (springs.Length == expectedSprings)
        {
            return MatchCharacter("", sequences, cache);
        }
        else if (springs[expectedSprings] == '#')
        {
            return 0;
        }
        else
        {
            return Compute(springs[(expectedSprings + 1)..], sequences, cache);
        }
    }

    private static Row Unfold(Row input, int repeat)
    {
        var row = string.Join('?', Enumerable.Repeat(input.Springs, repeat).ToArray());
        var sequences = new List<int>();
        for (var i = 0; i < repeat; i++)
        {
            sequences.AddRange(input.Sequences);
        }

        return new(row, sequences.ToArray());
    }
}
