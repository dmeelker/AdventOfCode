namespace AoC;


internal class Program
{
    private static void Main(string[] args)
    {
        var input = Parser.Parse(File.ReadAllLines("input.txt"));

        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part1: {part1} Part2: {part2}");

        if (part1 != 5695 || part2 != 2434)
        {
            throw new Exception();
        }
    }

    private static long Part1(Move[] input)
    {
        return SimulateRope(input, 2);
    }

    private static long Part2(Move[] input)
    {
        return SimulateRope(input, 10);
    }

    private static long SimulateRope(Move[] input, int knotCount)
    {
        var visitedLocations = new HashSet<Vector>();
        var knots = Enumerable.Repeat(new Vector(0, 0), knotCount).ToArray();

        foreach (var move in input)
        {
            for (var step = 0; step < move.Distance; step++)
            {
                knots[0] = move.Direction switch
                {
                    "U" => knots[0].Add(new(0, -1)),
                    "D" => knots[0].Add(new(0, 1)),
                    "L" => knots[0].Add(new(-1, 0)),
                    "R" => knots[0].Add(new(1, 0)),
                    _ => throw new Exception()
                };

                for (var i = 1; i < knots.Length; i++)
                {
                    var delta = new Vector(knots[i - 1].X - knots[i].X, knots[i - 1].Y - knots[i].Y);

                    if (Math.Abs(delta.X) + Math.Abs(delta.Y) > 2)
                    {
                        knots[i] = knots[i].Add(new(Math.Sign(delta.X), Math.Sign(delta.Y)));
                    }
                    else if (Math.Abs(delta.X) > 1)
                    {
                        knots[i] = knots[i].Add(new(Math.Sign(delta.X), 0));
                    }
                    else if (Math.Abs(delta.Y) > 1)
                    {
                        knots[i] = knots[i].Add(new(0, Math.Sign(delta.Y)));
                    }

                }
                visitedLocations.Add(knots.Last());
            }
        }

        return visitedLocations.Count;
    }
}