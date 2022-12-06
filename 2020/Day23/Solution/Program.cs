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

        public static string Part1(int[] input)
        {
            var cups = Simulate(input, 100);

            return SerializeValues(cups);
        }

        public static long Part2(int[] input)
        {
            var cups = input.ToList();
            cups.AddRange(Enumerable.Range(input.Max() + 1, 1_000_000 - cups.Count));

            var result = Simulate(cups, 10_000_000);
            var start = result.IndexOf(1);
            var nextCups = GetRange(result, start + 1, 2).ToArray();

            return (long)nextCups[0] * nextCups[1];
        }

        public static List<int> Simulate(IEnumerable<int> input, int steps)
        {
            var cups = new LinkedList<int>(input);
            var lookup = GenerateLookup(cups);
            var maxValue = cups.Max();

            var currentNode = cups.First;

            for (var i = 0; i < steps; i++)
            {
                var collectedCups = RemoveCups(cups, currentNode, 3);
                int targetCupValue = FindTarget(lookup, currentNode, maxValue);
                InsertCups(cups, lookup[targetCupValue], collectedCups);

                currentNode = currentNode.Next ?? cups.First;
            }

            return cups.ToList();
        }

        private static LinkedListNode<int>[] RemoveCups(LinkedList<int> cups, LinkedListNode<int> currentCup, int count)
        {
            var collectedCups = GetRange(cups, currentCup.Next ?? cups.First, count).ToArray();
            foreach (var c in collectedCups)
                cups.Remove(c);

            return collectedCups;
        }

        private static int FindTarget(Dictionary<int, LinkedListNode<int>> lookup, LinkedListNode<int> currentNode, int maxValue)
        {
            var targetCup = currentNode.Value > 1 ? currentNode.Value - 1 : maxValue;

            while (lookup[targetCup].List == null)
                targetCup = targetCup - 1 > 0 ? targetCup - 1 : maxValue;

            return targetCup;
        }

        private static void InsertCups(LinkedList<int> cups, LinkedListNode<int> target, IEnumerable<LinkedListNode<int>> collectedCups)
        {
            foreach (var c in collectedCups)
            {
                cups.AddAfter(target, c);
                target = c;
            }
        }

        public static Dictionary<int, LinkedListNode<int>> GenerateLookup(LinkedList<int> values)
        {
            var lookup = new Dictionary<int, LinkedListNode<int>>();

            var node = values.First;
            while (node != null)
            {
                lookup[node.Value] = node;
                node = node.Next;
            }

            return lookup;
        }

        public static string SerializeValues(List<int> cups)
        {
            var start = cups.IndexOf(1) + 1;
            if (start >= cups.Count)
                start = 0;

            return GetRange(cups, start, cups.Count - 1)
                .Select(v => v.ToString())
                .Aggregate((v1, v2) => v1 + v2);
        }


        public static IEnumerable<int> GetRange(List<int> input, int start, int length)
        {
            var arrayIndex = start;

            for(var i=0; i<length; i++)
            {
                if (arrayIndex >= input.Count)
                    arrayIndex = 0;

                yield return input[arrayIndex];

                arrayIndex++;                
            }
        }

        public static IEnumerable<LinkedListNode<int>> GetRange(LinkedList<int> input, LinkedListNode<int> start, int length)
        {
            var arrayIndex = start;
            var node = start;

            for (var i = 0; i < length; i++)
            {
                yield return node;
                node = node.Next;
                if (node == null)
                    node = input.First;
            }
        }
    }
}
