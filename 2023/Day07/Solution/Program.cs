using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public enum HandType
{
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}

public record Hand(char[] Cards, int Bid)
{
    public char[]? JokeredHand { get; set; } = null;
    public char[] EffectiveHand => JokeredHand != null ? JokeredHand : Cards;
};

public class Program
{
    private static readonly string _cardOrderPart1 = "23456789TJQKA";
    private static readonly string _cardOrderPart2 = "J23456789TQKA";
    private static readonly char[] _allNonJokerCards = _cardOrderPart1.ToCharArray().Where(c => c != 'J').ToArray();

    static void Main(string[] args)
    {
        var input = Parser.ParseInput(File.ReadAllText("input.txt"));
        var part1 = Part1(input);
        var part2 = Part2(input);

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 248836197L || part2 != 251195607L)
        {
            throw new Exception();
        }
    }

    public static long Part1(Hand[] input)
    {
        return input.OrderBy(hand => CalculateHandValue(hand, _cardOrderPart1))
            .Select((hand, index) => (long)hand.Bid * (index + 1))
            .Sum();
    }

    public static long Part2(Hand[] input)
    {
        return input.Select(FindBestHand)
            .OrderBy(hand => CalculateHandValue(hand, _cardOrderPart2))
            .Select((hand, index) => (long)hand.Bid * (index + 1))
            .Sum();
    }

    public static Hand FindBestHand(Hand hand)
    {
        var jokerLocations = hand.Cards.Select((card, index) => new { card, index }).Where(c => c.card == 'J').Select(c => c.index).ToArray();
        var possibleHands = new List<char[]>();
        var permutation = Copy(hand.Cards);

        GenerateHands(permutation, jokerLocations, 0, possibleHands);

        return possibleHands
            .Select(g => new Hand(hand.Cards, hand.Bid) { JokeredHand = g })
            .OrderBy(h => CalculateHandValue(h, _cardOrderPart2))
            .Last();
    }

    private static void GenerateHands(char[] permutation, int[] jokerLocations, int jokerIndex, List<char[]> possibleHands)
    {
        if (jokerIndex == jokerLocations.Length)
        {
            possibleHands.Add(Copy(permutation));
            return;
        }

        foreach (var card in _allNonJokerCards)
        {
            permutation[jokerLocations[jokerIndex]] = card;
            GenerateHands(permutation, jokerLocations, jokerIndex + 1, possibleHands);
        }
    }

    private static char[] Copy(char[] cards)
    {
        var copy = new char[5];
        Array.Copy(cards, copy, 5);
        return copy;
    }

    public static long CalculateHandValue(Hand hand, string cardOrder)
    {
        var baseValue = (int)GetHandType(hand) * 10_000_000_000L;

        var cardsValue = hand.Cards
            .Select((card, index) => (cardOrder.IndexOf(card) + 1) * (int)Math.Pow(13, 5 - index))
            .Sum();

        return baseValue + cardsValue;
    }

    public static HandType GetHandType(Hand hand)
    {
        return GetHandPattern(hand.EffectiveHand) switch
        {
            "11111" => HandType.FiveOfAKind,
            "11112" => HandType.FourOfAKind,
            "11122" => HandType.FullHouse,
            "11123" => HandType.ThreeOfAKind,
            "11223" => HandType.TwoPairs,
            "11234" => HandType.OnePair,
            _ => HandType.HighCard,
        };
    }

    private static string GetHandPattern(char[] cards)
    {
        return new string(cards
            .GroupBy(c => c)
            .OrderByDescending(group => group.Count())
            .SelectMany((group, index) => Enumerable.Repeat(index + 1, group.Count()))
            .SelectMany(count => count.ToString())
            .ToArray());
    }
}
