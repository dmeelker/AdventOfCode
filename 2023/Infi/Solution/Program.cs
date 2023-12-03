using AoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public record Circle(Vector Center, double Radius)
    {
        public bool Contains(Vector point)
        {
            double distance = point.Subtract(Center).Length;
            return distance <= Radius;
        }

        public static Circle CircleFromTwoPoints(Vector a, Vector b)
        {
            Vector center = new Vector((a.X + b.X) / 2, (a.Y + b.Y) / 2);
            double radius = Math.Sqrt(Math.Pow(a.X - center.X, 2) + Math.Pow(a.Y - center.Y, 2));
            return new Circle(center, radius);
        }

        public static Circle CircleFromThreePoints(Vector a, Vector b, Vector c)
        {
            double ma = (b.Y - a.Y) / (b.X - a.X);
            double mb = (c.Y - b.Y) / (c.X - b.X);

            double centerX = (ma * mb * (a.Y - c.Y) + mb * (a.X + b.X) - ma * (b.X + c.X)) / (2 * (mb - ma));
            double centerY = (-1 / ma) * (centerX - (a.X + b.X) / 2) + (a.Y + b.Y) / 2;

            double radius = Math.Sqrt(Math.Pow(a.X - centerX, 2) + Math.Pow(a.Y - centerY, 2));
            return new Circle(new Vector(centerX, centerY), radius);
        }
    }


    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("testinput.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

            Console.Clear();
        }

        public static int Part1(List<List<Vector>> input)
        {
            var values = new List<double>();
            foreach (var package in input)
            {
                var max = 0.0;
                foreach (var location in package)
                {
                    var length = Math.Abs(location.Length);
                    max = Math.Max(length, max);
                }
                values.Add(max);
            }
            return (int)values.Sum();
        }

        public static int Part2(List<List<Vector>> input)
        {
            var values = new List<double>();
            foreach (var package in input)
            {
                var circle = MinimumBoundingCircle(package);
                values.Add(circle.Radius);
            }
            return (int)values.Sum();
        }

        static Circle MinimumBoundingCircle(List<Vector> points)
        {
            var permutations = GenerateVectorPairs(points);
            var circles = new List<Circle>();

            foreach (var permutation in permutations)
            {
                var circle = Circle.CircleFromThreePoints(permutation.Item1, permutation.Item2, permutation.Item3);
                Console.WriteLine($"Center: {circle.Center} Radius: {circle.Radius}");
                var remainingPoints = points.Except(new[] { permutation.Item1, permutation.Item2, permutation.Item3 });

                if (remainingPoints.All(circle.Contains))
                {
                    circles.Add(circle);
                }
            }

            return circles.OrderBy(c => c.Radius).First();
        }

        static List<Tuple<Vector, Vector, Vector>> GenerateVectorPairs(List<Vector> vectors)
        {
            var vectorPairs = new List<Tuple<Vector, Vector, Vector>>();

            for (int i = 0; i < vectors.Count - 1; i++)
            {
                for (int j = i + 1; j < vectors.Count; j++)
                {
                    for (int y = j + 1; y < vectors.Count; y++)
                    {
                        vectorPairs.Add(new Tuple<Vector, Vector, Vector>(vectors[i], vectors[j], vectors[y]));
                    }
                }
            }

            return vectorPairs;
        }

        static Circle MinimumBoundingCircle2(List<Vector> points)
        {
            // Get all permutations of points


            // Randomize the order of the points to improve the algorithm's performance
            Random rand = new Random();
            List<Vector> shuffledPoints = new List<Vector>(points);
            for (int i = shuffledPoints.Count - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                Vector temp = shuffledPoints[i];
                shuffledPoints[i] = shuffledPoints[j];
                shuffledPoints[j] = temp;
            }

            // Use Welzl's algorithm to find the smallest bounding circle
            return WelzlAlgorithm(shuffledPoints, new List<Vector>());
        }



        static Circle WelzlAlgorithm(List<Vector> points, List<Vector> boundary)
        {
            if (points.Count == 0 || boundary.Count == 3)
            {
                return MinCircleTrivial(boundary);
            }

            Vector randomPoint = points[^1];
            points.RemoveAt(points.Count - 1);

            Circle minimumCircle = WelzlAlgorithm(new List<Vector>(points), new List<Vector>(boundary));

            if (!IsPointInCircle(randomPoint, minimumCircle))
            {
                boundary.Add(randomPoint);
                minimumCircle = WelzlAlgorithm(new List<Vector>(points), new List<Vector>(boundary));
                boundary.RemoveAt(boundary.Count - 1);
            }

            points.Add(randomPoint);
            return minimumCircle;
        }

        static Circle MinCircleTrivial(List<Vector> points)
        {
            if (points.Count == 0)
            {
                return new Circle(new Vector(0, 0), 0);
            }
            else if (points.Count == 1)
            {
                return new Circle(points[0], 0);
            }
            else if (points.Count == 2)
            {
                return CircleFromTwoPoints(points[0], points[1]);
            }

            // Points are sorted in a circular order
            return CircleFromThreePoints(points[0], points[1], points[2]);
        }

        static Circle CircleFromTwoPoints(Vector a, Vector b)
        {
            Vector center = new Vector((a.X + b.X) / 2, (a.Y + b.Y) / 2);
            double radius = Math.Sqrt(Math.Pow(a.X - center.X, 2) + Math.Pow(a.Y - center.Y, 2));
            return new Circle(center, radius);
        }

        static Circle CircleFromThreePoints(Vector a, Vector b, Vector c)
        {
            double ma = (b.Y - a.Y) / (b.X - a.X);
            double mb = (c.Y - b.Y) / (c.X - b.X);

            double centerX = (ma * mb * (a.Y - c.Y) + mb * (a.X + b.X) - ma * (b.X + c.X)) / (2 * (mb - ma));
            double centerY = (-1 / ma) * (centerX - (a.X + b.X) / 2) + (a.Y + b.Y) / 2;

            double radius = Math.Sqrt(Math.Pow(a.X - centerX, 2) + Math.Pow(a.Y - centerY, 2));
            return new Circle(new Vector(centerX, centerY), radius);
        }

        static bool IsPointInCircle(Vector point, Circle circle)
        {
            double distance = Math.Sqrt(Math.Pow(point.X - circle.Center.X, 2) + Math.Pow(point.Y - circle.Center.Y, 2));
            return distance <= circle.Radius;
        }
    }
}
