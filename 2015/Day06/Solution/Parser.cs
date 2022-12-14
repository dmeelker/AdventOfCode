using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public enum Action
    {
        TurnOn,
        TurnOff,
        Toggle
    }

    public record Instruction(Action Action, Point From, Point To);

    public static class Parser
    {
        public static List<Instruction> ParseInput(string input)
        {
            return input.ToLines().Select(ParseInstruction).ToList();
        }

        private static Instruction ParseInstruction(string line)
        {
            Action action;
            if (line.StartsWith("turn on "))
            {
                line = line.StripPrefix("turn on ");
                action = Action.TurnOn;
            }
            else if (line.StartsWith("turn off "))
            {
                line = line.StripPrefix("turn off ");
                action = Action.TurnOff;
            }
            else if (line.StartsWith("toggle "))
            {
                line = line.StripPrefix("toggle ");
                action = Action.Toggle;
            }
            else
            {
                throw new Exception();
            }

            var sections = line.Split(" through ");
            var from = ParsePoint(sections[0]);
            var to = ParsePoint(sections[1]);
            return new(action, from, to);
        }

        private static Point ParsePoint(string input)
        {
            var values = input.ParseIntArray();
            return new(values[0], values[1]);
        }
    }
}
