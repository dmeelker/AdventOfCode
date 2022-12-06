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

        public static int Part1(int[][] input)
        {
            var stacks = input.Select(s => new Queue<int>(s)).ToArray();

            while(stacks.All(s => s.Count > 0))
            {
                var drawnCards = stacks.Select(s => s.Dequeue()).ToArray();

                var winner = IndexOfMax(drawnCards[0], drawnCards[1]);
                stacks[winner].Enqueue(drawnCards[winner]);
                stacks[winner].Enqueue(drawnCards[1 - winner]);
            }

            return CalculateScore(stacks.Single(s => s.Count > 0).ToArray());
        }

        public static int Part2(int[][] input)
        {
            return CalculateScore(PlayGame(input).stack.ToArray());
        }

        public static (int winner, int[] stack) PlayGame(int[][] input)
        {
            var stacks = input.Select(s => new Queue<int>(s)).ToArray();
            var previousStates = new HashSet<string>();
                
            while (stacks.All(s => s.Count > 0))
            {
                var state = HashState(stacks);
                if (previousStates.Contains(state))
                    return (0, stacks[0].ToArray()); // Player 1 wins
                else
                    previousStates.Add(state);

                var drawnCards = stacks.Select(s => s.Dequeue()).ToArray();
                int winner = IndexOfMax(drawnCards[0], drawnCards[1]);

                if (ElementsEqualOrLarger(stacks.Select(s => s.Count).ToArray(), drawnCards))
                    winner = PlayGame(stacks.Select((s, i) => s.Take(drawnCards[i]).ToArray()).ToArray()).winner; // Sub game!

                stacks[winner].Enqueue(drawnCards[winner]);
                stacks[winner].Enqueue(drawnCards[1 - winner]);
            }

            return stacks.Select((s, i) => (index: i, stack: s.ToArray()))
                .Single(s => s.stack.Length != 0);
        }

        public static string HashState(IEnumerable<IEnumerable<int>> stacks)
        {
            return string.Join('#', stacks.Select(s => string.Join(',', s.Select(c => c.ToString()))));
        }

        public static int CalculateScore(int[] cards)
        {
            return cards.Select((value, index) => value * (cards.Length - index)).Sum();
        }

        public static int IndexOfMax(params int[] values)
        {
            return values.Select((v, i) => (value: v, index: i))
                .OrderByDescending(p => p.value)
                .First()
                .index;
        }

        public static bool ElementsEqualOrLarger(int[] a1, int[] a2)
        {
            return a1.Select((v, i) => (value: v, index: i))
                .All(p => p.value >= a2[p.index]);
        }
    } 
}
