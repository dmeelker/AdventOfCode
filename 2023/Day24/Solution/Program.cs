using System;
using System.IO;
using System.Linq;

namespace Solution;

public record Vector2D(long X, long Y)
{
}

public record Vector3D(long X, long Y, long Z)
{
    public static Vector3D operator +(Vector3D a, Vector3D b)
    {
        return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3D operator -(Vector3D a, Vector3D b)
    {
        return new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3D operator *(Vector3D v, long scalar)
    {
        return new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }
}

public record Line2D(Vector2D Start, Vector2D Direction);

public record Line3D(Vector3D Start, Vector3D Direction);

public class Program
{
    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("testinput.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
    }

    public static int Part1(Line3D[] input)
    {
        var min = 7;// 200_000_000_000_000L;
        var max = 27;// 400_000_000_000_000L;
        var answer = 0;

        var lines = input.Select(line => new Line2D(new Vector2D(line.Start.X, line.Start.Y), new Vector2D(line.Direction.X, line.Direction.Y))).ToArray();
        var counter = 0;

        foreach (var line1 in lines)
        {
            foreach (var line2 in lines)
            {
                if (line1 == line2)
                {
                    continue;
                }

                var denom = (line1.Direction.X * line2.Direction.Y) - (line1.Direction.Y * line2.Direction.X);
                if (denom == 0)
                {
                    if (line1.Start.X == line2.Start.X && line1.Start.Y == line2.Start.Y)
                    {
                        if (line1.Start.X >= min && line1.Start.X <= max && line1.Start.Y >= min && line1.Start.Y <= max)
                        {
                            answer++;
                        }
                    }
                    continue;
                }
                var numer1 = ((line2.Start.X - line1.Start.X) * line2.Direction.Y) - ((line2.Start.Y - line1.Start.Y) * line2.Direction.X);
                var numer2 = ((line1.Start.X - line2.Start.X) * line1.Direction.Y) - ((line1.Start.Y - line2.Start.Y) * line1.Direction.X);
                var intersectionX = (numer1 / denom) * line1.Direction.X + line1.Start.X;
                var intersectionY = (numer1 / denom) * line1.Direction.Y + line1.Start.Y;
                if (intersectionX >= min && intersectionX <= max && intersectionY >= min && intersectionY <= max)
                {
                    if ((numer1 / denom) > 0 && (numer2 / denom) < 0)
                    {
                        answer++;
                    }
                }
            }
        }

        return counter;
    }

    public static int Part2(Line3D[] input)
    {
        return 0;
    }





    //static Vector2 FindIntersectionPoint(Vector2 start1, Vector2 direction1, Vector2 start2, Vector2 direction2)
    //{
    //    float t = Vector2.Dot(start2 - start1, new Vector2(-direction2.Y, direction2.X)) / Vector2.Dot(direction1, new Vector2(-direction2.Y, direction2.X));
    //    return start1 + t * direction1;
    //}

    //static Vector3D FindIntersectionPoint(Line3D line1, Line3D line2)
    //{
    //    // Solve the system of equations
    //    var t = SolveSystem(line1.Start.X, line1.Direction.X, line2.Start.X, line2.Direction.X);
    //    var s = SolveSystem(line1.Start.Y, line1.Direction.Y, line2.Start.Y, line2.Direction.Y);

    //    // Calculate the intersection point
    //    Vector3D intersectionPoint = line1.Start + line1.Direction * t;

    //    return intersectionPoint;
    //}

    //static long SolveSystem(long p1, long d1, long p2, long d2)
    //{
    //    // Solve the system of equations for a single parameter
    //    long determinant = d1 * 1 - d2 * 1;

    //    if (Math.Abs(determinant) < 1e-10)
    //    {
    //        throw new InvalidOperationException("Lines are parallel; no unique intersection point.");
    //    }

    //    // Calculate the parameter
    //    long t = (p2 - p1) / determinant;

    //    return t;
    //}
}
