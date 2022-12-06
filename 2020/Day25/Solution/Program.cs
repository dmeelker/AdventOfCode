using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);

            Console.WriteLine($"Part 1: {part1}");
        }

        public static long Part1(int[] input)
        {
            var loopSizes = input.Select(publicKey => FindLoopSize(publicKey, 7));
            var keys = input.Reverse()
                .Zip(loopSizes).Select(pair => Encrypt(pair.First, pair.Second))
                .ToArray();

            Debug.Assert(keys[0] == keys[1]);

            return keys[0];
        }

        public static int FindLoopSize(int publicKey, int subject)
        {
            var value = 1;
            var i = 0;

            while(value != publicKey)
            {
                value = (value * subject) % 20201227;
                i++;
            }

            return i;
        }

        public static long Encrypt( int subject, int loopSize)
        {
            var value = 1L;

            foreach(var _ in Enumerable.Range(0, loopSize))
            {
                value = (value * subject) % 20201227;
            }

            return value;
        }
    }
}
