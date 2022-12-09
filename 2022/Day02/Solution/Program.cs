using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        private const char RESULT_LOSE = 'X';
        private const char RESULT_DRAW = 'Y';
        private const char RESULT_WIN = 'Z';

        public static readonly Tuple<HandShape, HandShape>[] _winCombinations = new[] {
            Tuple.Create(HandShape.Rock, HandShape.Scissor),
            Tuple.Create(HandShape.Paper, HandShape.Rock),
            Tuple.Create(HandShape.Scissor, HandShape.Paper),
        };

        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));

            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(IEnumerable<Tuple<char, char>> input)
        {
            var rounds = input.Select(entry => new Round(ParseShape(entry.Item1), ParseMyShape(entry.Item2)));

            return GetTotalScore(rounds);
        }

        private static int GetTotalScore(IEnumerable<Round> rounds)
        {
            return rounds.Select(round => round.GetScore()).Sum();
        }

        private static HandShape ParseMyShape(char input)
        {
            return ParseShape((char)(input - ('X' - 'A')));
        }

        private static HandShape ParseShape(char input)
        {
            return input switch
            {
                'A' => HandShape.Rock,
                'B' => HandShape.Paper,
                'C' => HandShape.Scissor,
                _ => throw new Exception()
            };
        }

        public static int Part2(IEnumerable<Tuple<char, char>> input)
        {
            var rounds = input.Select(entry => new Round(
                ParseShape(entry.Item1),
                GetMyMove(ParseShape(entry.Item1), entry.Item2)
            ));

            return GetTotalScore(rounds);
        }

        private static HandShape GetMyMove(HandShape theirMove, char requiredResult)
        {
            if (requiredResult == RESULT_LOSE)
            {
                return GetLoseShape(theirMove);
            }
            else if (requiredResult == RESULT_DRAW)
            {
                return theirMove;
            }
            else if (requiredResult == RESULT_WIN)
            {
                return GetWinShape(theirMove);
            }
            else
            {
                throw new Exception();
            }
        }

        public static HandShape GetWinShape(HandShape theirs)
        {
            return _winCombinations.First(combination => combination.Item2 == theirs).Item1;
        }

        private static HandShape GetLoseShape(HandShape theirs)
        {
            return _winCombinations.First(combination => combination.Item1 == theirs).Item2;
        }
    }

    public enum HandShape
    {
        Rock,
        Paper,
        Scissor
    }

    public record Round(HandShape theirs, HandShape mine)
    {
        public int GetScore()
        {
            var outcome = CalculateOutcome();
            var handScore = GetShapeScore(mine);
            return outcome + handScore;
        }

        private int CalculateOutcome()
        {
            if (IsDraw)
            {
                return 3;
            }
            else if (IsWin)
            {
                return 6;
            }
            else
            {
                return 0;
            }
        }

        private int GetShapeScore(HandShape myMove)
        {
            return myMove switch
            {
                HandShape.Rock => 1,
                HandShape.Paper => 2,
                HandShape.Scissor => 3,
                _ => throw new Exception()
            };
        }

        private bool IsDraw => theirs == mine;
        private bool IsWin => Program.GetWinShape(theirs) == mine;
    }
}
