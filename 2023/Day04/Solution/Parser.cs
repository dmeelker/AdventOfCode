using Shared;
using System;
using System.Linq;

namespace Solution
{
    public record Card(int Index, int Id, int[] WinningNumbers, int[] MyNumbers)
    {
        public Lazy<int> WinningNumberCount => new Lazy<int>(() => WinningNumbers.Intersect(MyNumbers).Count());
    };

    public static class Parser
    {
        public static Card[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseCard)
                .ToArray();
        }

        private static Card ParseCard(string line, int index)
        {
            var sections = line.Split(':', '|');

            var id = int.Parse(sections[0].StripPrefix("Card "));
            var winningNumbers = ParseNumbers(sections[1]);
            var myNumbers = ParseNumbers(sections[2]);
            return new Card(index, id, winningNumbers, myNumbers);
        }

        private static int[] ParseNumbers(string input)
        {
            return input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
        }
    }
}
