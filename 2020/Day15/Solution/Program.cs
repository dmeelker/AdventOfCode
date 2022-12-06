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
            var part1 = Solve(input, 2020);
            var part2 = Solve(input, 30000000);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static long Solve(long[] input, long target)
        {
            return GenerateNumbers(input)
                .Select((value, index) => (value: value, index: index + 1))
                .SkipWhile(pair => pair.index < target)
                .First().value;
        }

        public static IEnumerable<long> GenerateNumbers(long[] input)
        {
            var numbers = input.Select((number, index) => (number, index)).ToDictionary(value => value.number, value => new NumberKnowledge(value.number, value.index));
            var lastNumber = input.Last();
            var numberCount = input.Length;

            foreach (var number in input)
                yield return number;

            while(true)
            {
                long number = numbers[lastNumber].SecondToLastIndex.HasValue ? numbers[lastNumber].Difference : 0;

                AddOrUpdateNumber(number);

                lastNumber = number;
                numberCount++;

                yield return number;
            }

            void AddOrUpdateNumber(long number)
            {
                if (numbers.ContainsKey(number))
                    numbers[number].Update(numberCount);
                else
                    numbers[number] = new NumberKnowledge(number, numberCount);
            }
        }
    }

    public class NumberKnowledge
    {
        public long Number { get; }
        public int? SecondToLastIndex { get; set; }
        public int LastIndex { get; set; }
        public int Difference => LastIndex - SecondToLastIndex.Value;

        public NumberKnowledge(long number, int lastIndex)
        {
            Number = number;
            LastIndex = lastIndex;
        }

        public void Update(int lastIndex)
        {
            SecondToLastIndex = LastIndex;
            LastIndex = lastIndex;
        }
    }
}
