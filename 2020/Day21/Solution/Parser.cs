using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public static class Parser
    {
        public static Food[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split("(contains "))
                .Select(parts => new Food
                {
                    Ingredients = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Allergens = parts[1].Substring(0, parts[1].Length - 1).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList()
                }).ToArray();
        }
    }

    public class Food
    {
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }

        public override string ToString()
        {
            return Ingredients.Aggregate((i1, i2) => i1 + ", " + i2) + " => " + (Allergens.Count() > 0 ? Allergens.Aggregate((i1, i2) => i1 + ", " + i2) : "");
        }
    }
}
