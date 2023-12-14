using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public record State(string Key, MirrorField Field);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input.Clone());
        var part2 = Part2(input.Clone());

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 108955 || part2 != 106689)
        {
            throw new Exception();
        }
    }

    public static int Part1(MirrorField field)
    {
        field.RollUp();
        return field.CalculateScore();
    }

    public static int Part2(MirrorField field)
    {
        var states = new List<State>();
        var target = 1_000_000_000;

        while (true)
        {
            var key = field.StateKey();

            var previousState = states
                .Select((State, Index) => new { State, Index })
                .FirstOrDefault(state => state.State.Key == key);

            if (previousState != null)
            {
                // Cycle found
                var nonRepeatingStateCount = previousState.Index;
                var cycleLength = states.Count - nonRepeatingStateCount;
                var endStateIndex = (target - nonRepeatingStateCount) % cycleLength;
                field = states[nonRepeatingStateCount + endStateIndex].Field;
                break;
            }

            states.Add(new State(key, field.Clone()));

            for (var i = 0; i < 4; i++)
            {
                field.RollUp();
                field.RotateRight();
            }
        }

        return field.CalculateScore();
    }
}