using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public record InputEntry(char Direction, int Distance, string Color);
public record Instruction(Point Direction, int Distance);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Solve(input.Select(CreatePart1Instruction));
        var part2 = Solve(input.Select(CreatePart2Instruction));

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 36807L || part2 != 48797603984357L)
        {
            throw new Exception();
        }
    }

    private static Instruction CreatePart1Instruction(InputEntry entry)
    {
        var direction = GetDirectionPart1(entry.Direction);
        var distance = entry.Distance;

        return new Instruction(direction, distance);
    }

    private static Instruction CreatePart2Instruction(InputEntry entry)
    {
        var direction = GetDirectionPart2(entry.Color[6]);
        var distance = Convert.ToInt32(entry.Color.Substring(1, 5), 16);

        return new Instruction(direction, distance);
    }

    private static long Solve(IEnumerable<Instruction> input)
    {
        var vertices = new List<Point>() { new Point(0, 0) };
        var location = new Point(0, 0);
        var totalDistance = 0;

        foreach (var instruction in input)
        {
            location = location.Add(instruction.Direction.Multiply(instruction.Distance));
            vertices.Add(location);
            totalDistance += instruction.Distance;
        }

        return ShoelaceFormula(vertices, totalDistance);
    }

    static long ShoelaceFormula(List<Point> vertices, int circumference)
    {
        long area = 0;

        for (int i = 0; i < vertices.Count - 1; i++)
        {
            var j = i + 1;
            area += ((long)vertices[i].X * vertices[j].Y) - ((long)vertices[i].Y * vertices[j].X);
        }

        return (Math.Abs(area) / 2) + (circumference / 2) + 1;
    }

    private static Point GetDirectionPart1(char dir)
    {
        return dir switch
        {
            'U' => Point.Up,
            'D' => Point.Down,
            'L' => Point.Left,
            'R' => Point.Right,
            _ => throw new Exception("Invalid direction")
        };
    }

    private static Point GetDirectionPart2(char dir)
    {
        return dir switch
        {
            '3' => Point.Up,
            '1' => Point.Down,
            '2' => Point.Left,
            '0' => Point.Right,
            _ => throw new Exception("Invalid direction")
        };
    }
}
