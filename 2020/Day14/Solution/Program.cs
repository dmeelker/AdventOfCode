using System;
using System.Collections.Generic;
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
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static long Part1(Block[] input)
        {
            var memory = new Dictionary<int, long>();

            foreach (var block in input)
            {
                foreach (var operation in block.Operations)
                {
                    var value = ApplyPart1BitMask(operation.Value, block.Mask);
                    memory[operation.Address] = Convert.ToInt64(value, 2);
                }
            }

            return memory.Values.Sum();
        }

        private static string ApplyPart1BitMask(long value, string mask)
        {
            var valueBits = LeftPad(Convert.ToString(value, 2), mask.Length);

            return new String(mask.ToCharArray().Select((chr, i) => chr switch
            {
                '1' => '1',
                '0' => '0',
                'X' => valueBits[i],
                _ => throw new Exception()
            }).ToArray());
        }

        public static long Part2(Block[] input)
        {
            var memory = new Dictionary<long, long>();

            foreach (var block in input)
            {
                foreach (var operation in block.Operations)
                {
                    var addressMask = ApplyPart2BitMask(operation.Address, block.Mask);

                    foreach (var value in GeneratePermutations(addressMask))
                    {
                        var address = Convert.ToInt64(value, 2);
                        memory[address] = operation.Value;
                    }
                }
            }

            return memory.Values.Sum();
        }
        private static IEnumerable<string> GeneratePermutations(string input)
        {
            var permutations = new List<string>();
            var floatingBits = input.Select((chr, index) => (chr: chr, index: index))
                .Where(chr => chr.chr == 'X')
                .Select(chr => chr.index)
                .ToArray();

            GeneratePermutations(input.ToCharArray(), floatingBits, 0, permutations);

            return permutations;
        }

        private static void GeneratePermutations(char[] input, int[] floatingBits, int bitIndex, List<string> permutations)
        {
            input[floatingBits[bitIndex]] = '1';
            GeneratePermutation();

            input[floatingBits[bitIndex]] = '0';
            GeneratePermutation();

            void GeneratePermutation()
            {
                if (bitIndex == floatingBits.Length - 1)
                    permutations.Add(new String(input));
                else
                    GeneratePermutations(input, floatingBits, bitIndex + 1, permutations);
            }
        }

        private static string ApplyPart2BitMask(long value, string mask)
        {
            var valueBits = LeftPad(Convert.ToString(value, 2), mask.Length);

            return new String(mask.ToCharArray().Select((chr, i) => chr switch
            {
                '1' => '1',
                '0' => valueBits[i],
                'X' => 'X',
                _ => throw new Exception()
            }).ToArray());
        }

        private static string LeftPad(string input, int length)
        {
            while(input.Length < length)
                input = "0" + input;

            return input;
        }
    }

    public class Block
    {
        public string Mask { get; set; }
        public Operation[] Operations { get; set; }
    }

    public class Operation
    {
        public int Address { get; set; }
        public long Value { get; set; }

        public Operation(int address, long value)
        {
            Address = address;
            Value = value;
        }
    }
}