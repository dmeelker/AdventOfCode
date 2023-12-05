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

        public static int Part1(Card[] cards)
        {
            return cards.Sum(card =>
                card.MyNumbers
                    .Where(number => card.WinningNumbers.Contains(number))
                    .Aggregate(0, (score, number) => score == 0 ? 1 : score * 2)
            );
        }

        public static int Part2(Card[] input)
        {
            var totalCardCount = 0;
            var cardsToProcess = new Stack<Card>(input.Reverse());

            while (cardsToProcess.Count > 0)
            {
                var card = cardsToProcess.Pop();
                totalCardCount++;

                for (var i = 0; i < card.WinningNumberCount.Value && i < input.Length; i++)
                {
                    cardsToProcess.Push(input[card.Index + i + 1]);
                }
            }

            return totalCardCount;
        }
    }
}
