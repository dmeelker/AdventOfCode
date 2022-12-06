using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Simulator
    {
        private const char FLOOR = '.';
        private const char FREE_SEAT = 'L';
        private const char OCCUPIED_SEAT = '#';

        public static char[,] SimulatePart1(char[,] input)
        {
            return input.Transform(point => Simulate(input, point, 4, 1));
        }

        public static char[,] SimulatePart2(char[,] input)
        {
            return input.Transform(point => Simulate(input, point, 5, int.MaxValue));
        }

        public static char Simulate(char[,] input, (int x, int y) point, int numberOfOccupiedSeatsToFlip, int maxDistance)
        {
            return input[point.x, point.y] switch {
                FLOOR => FLOOR,
                FREE_SEAT => CountOccupiedSeatsInSight(input, point, maxDistance) == 0 ? OCCUPIED_SEAT : FREE_SEAT,
                OCCUPIED_SEAT => CountOccupiedSeatsInSight(input, point, maxDistance) >= numberOfOccupiedSeatsToFlip ? FREE_SEAT : OCCUPIED_SEAT,
                _ => throw new Exception($"Unknown state: {input[point.x, point.y]}")
            };
        }

        public static int CountOccupiedSeatsInSight(char[,] input, (int x, int y) point, int maxDistance)
        {
            return GetAdjacentVectors().Count(vector => FindOccupiedSeatInSight(input, point, vector, maxDistance));
        }

        public static IEnumerable<(int x, int y)> GetAdjacentVectors()
        {
            return Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3), (x, y) => (x, y))
                .Where(point => !(point.x == 0 && point.y == 0));
        }

        public static bool FindOccupiedSeatInSight(char[,] input, (int x, int y) point, (int x, int y) vector, int maxDistance)
        {
            return TracePath(point, vector, maxDistance)
                .TakeWhile(cell => input.ContainsPoint(cell))
                .Select(cell => input[cell.x, cell.y])
                .SkipWhile(cell => cell == FLOOR)
                .FirstOrDefault() == OCCUPIED_SEAT;
        }

        public static IEnumerable<(int x, int y)> TracePath((int x, int y) point, (int x, int y) vector, int maxDistance)
        {
            return Enumerable.Range(1, maxDistance)
                .Select(distance => (point.x + (vector.x * distance), point.y + (vector.y * distance)));
        }
    }
}
