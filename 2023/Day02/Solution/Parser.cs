using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public record Cubes(int Quantity, string Color);
    public record Game(int Id, List<List<Cubes>> Sets);

    public static class Parser
    {
        public static Game[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseGame)
                .ToArray();
        }

        public static Game ParseGame(string line)
        {
            line = line.Substring("Game ".Length);
            var sections = line.Split(':');
            var id = int.Parse(sections[0]);

            var sets = sections[1].Split(';');

            var parsedSets = sets.Select(set => set.Split(", ").Select(ParseCubes).ToList()).ToList();

            return new Game(id, parsedSets);
        }

        public static Cubes ParseCubes(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new Cubes(int.Parse(parts[0]), parts[1].Trim());
        }
    }
}
