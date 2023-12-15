using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

record Instruction(string Label, char Operation, int? Argument);
record Lens(string Label, int FocalLength);

public class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 512283 || part2 != 215827)
        {
            throw new Exception();
        }
    }

    public static int Part1(string input)
    {
        return input.Split(',').Sum(Hash);
    }

    private static int Hash(string input)
    {
        return input.ToCharArray()
            .Select(c => (int)c)
            .Aggregate(0, (v1, v2) => ((v1 + v2) * 17) % 256);
    }

    public static int Part2(string input)
    {
        var instructions = input.Split(',').Select(ParseInstruction).ToArray();

        var boxes = Enumerable.Range(0, 256)
            .Select(i => new List<Lens>())
            .ToArray();

        foreach (var instruction in instructions)
        {
            var box = boxes[Hash(instruction.Label)];

            if (instruction.Operation == '=')
            {
                var lens = new Lens(instruction.Label, instruction.Argument!.Value);

                var existingLens = box.Find(l => l.Label == instruction.Label);
                box!.ReplaceOrAdd(existingLens, lens);
            }
            else
            {
                box.RemoveAll(l => l.Label == instruction.Label);
            }
        }

        return boxes.Select((box, boxIndex) =>
            box.Select((lens, lensIndex) => LensScore(boxIndex, lensIndex, lens.FocalLength)).Sum()
        ).Sum();
    }

    private static int LensScore(int boxIndex, int lensIndex, int focalLength)
    {
        return (1 + boxIndex) * (lensIndex + 1) * focalLength;
    }

    private static Instruction ParseInstruction(string input)
    {
        if (input.IndexOf('=') != -1)
        {
            var parts = input.Split('=');
            var label = parts[0];
            var focalLength = int.Parse(parts[1]);
            return new(label, '=', focalLength);
        }
        else
        {
            var label = input.Substring(0, input.Length - 1);
            return new(label, '-', null);
        }
    }
}