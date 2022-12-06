using System;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(line => long.Parse(line)).ToArray();
            var window = 25;

            var part1 = FindPart1(input, window);
            var part2 = FindPart2(input, part1);

            var part = input[part2.Item1..(part2.Item2 + 1)].OrderBy(value => value).ToArray();
            var part2Result = part.First() + part.Last();

            Console.WriteLine($"Part1: {part1} Part2: {part2Result}");
        }

        static long FindPart1(long[] input, int window)
        {
            for (int i = window; i < input.Length; i++)
            {
                if (!CheckPart1(input, i - window, window, input[i]))
                {
                    return input[i];
                }
            }

            throw new Exception("Not found");
        }

        static bool CheckPart1(long[] data, int windowStart, int windowLength, long searchNumber)
        {
            foreach(var n1 in Enumerable.Range(windowStart, windowLength).Select((n, i) => (n,i)))
            {
                foreach (var n2 in Enumerable.Range(windowStart, windowLength).Select((n, i) => (n, i)))
                {
                    if (n1.i == n2.i)
                        continue;

                    if(data[n1.n] + data[n2.n] == searchNumber)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static (int, int) FindPart2(long[] data, long searchNumber)
        {
            for (int n1 = 0; n1 < data.Length; n1++)
            {
                long sum = data[n1];

                for (int n2 = n1+1; n2 < data.Length; n2++)
                {
                    sum += data[n2];

                    if(sum == searchNumber)
                    {
                        return (n1, n2);
                    } else if(sum > searchNumber)
                    {
                        break;
                    }
                }
            }

            throw new Exception("Not found");
        }
    }
}
