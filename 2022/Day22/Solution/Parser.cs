using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solution
{
    public enum Direction
    {
        Right,
        Down,
        Left,
        Up,
    }

    public enum Turn
    {
        Right,
        Left
    }

    public class Input
    {
        public Grid<char> World { get; }
        public List<Instruction> Instructions { get; }

        public Input(Grid<char> world, List<Instruction> instructions)
        {
            World = world;
            Instructions = instructions;
        }
    }

    public enum InstructionType
    {
        Move,
        TurnLeft,
        TurnRight,
    }

    public record Instruction(InstructionType Type, int? Steps);

    public static class Parser
    {
        public static Input ParseInput(string input)
        {
            var sections = input.ToSections();
            return new Input(
                ParseWorld(sections[0]),
                ParseInstructions(sections[1])
                );
        }

        private static List<Instruction> ParseInstructions(string input)
        {
            var result = new List<Instruction>();

            var matches = Regex.Matches(input, "(\\d+|[RL])");
            return matches.Select(match => ParseInstruction(match.Groups[1].Value)).ToList();
        }

        private static Instruction ParseInstruction(string value)
        {
            if (value.IsNumber())
            {
                return new(InstructionType.Move, value.ParseInt());
            }
            else
            {
                return new(
                    value switch
                    {
                        "R" => InstructionType.TurnRight,
                        "L" => InstructionType.TurnLeft
                    },
                    null);
            }
        }

        private static Grid<char> ParseWorld(string v)
        {
            var lines = v.ToLines();
            var maxWidth = lines.Max(line => line.Length);
            lines = lines.Select(line => line.PadRight(maxWidth)).ToArray();
            var chars = lines.Select(line => line.ToCharArray()).ToArray();

            var world = new Grid<char>(chars[0].Length, chars.Length, ' ');

            for (var y = 0; y < chars.Length; y++)
            {
                for (var x = 0; x < chars[y].Length; x++)
                {
                    world.Set(x, y, chars[y][x]);
                }
            }

            return world;
        }
    }
}
