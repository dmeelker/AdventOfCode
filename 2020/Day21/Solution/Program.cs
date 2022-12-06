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
            var (part1, part2) = Solve(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static (int,string) Solve(Food[] input)
        {
            var allergenToIngredientMapping = ResolveAllergenMappings(input);

            var part1 = input.Sum(food => food.Ingredients.Count(i => !allergenToIngredientMapping.Values.Contains(i)));

            var part2 = allergenToIngredientMapping.Select(a => (allergen: a.Key, ingredient: a.Value))
                .OrderBy(i => i.allergen)
                .Select(i => i.ingredient)
                .Aggregate((i1, i2) => i1 + "," + i2);

            return (part1, part2);
        }

        private static Dictionary<string, string> ResolveAllergenMappings(Food[] input)
        {
            var allergens = GetAllergenIntersections(input);
            return ReduceIntersections(allergens).ToDictionary(a => a.Key, a => a.Value.Single());
        }

        private static Dictionary<string, HashSet<string>> GetAllergenIntersections(Food[] input)
        {
            return input.SelectMany(food => food.Allergens.Select(a => (allergen: a, ingredients: food.Ingredients.ToHashSet())))
                .GroupBy(allergen => allergen.allergen, allergen => allergen.ingredients)
                .ToDictionary(group => group.Key, group => group.Aggregate((s1, s2) => s1.Intersect(s2).ToHashSet()));
        }

        private static Dictionary<string, HashSet<string>> ReduceIntersections(Dictionary<string, HashSet<string>> allergens)
        {
            while (allergens.Values.Any(a => a.Count != 1))
            {
                var singles = allergens.Where(a => a.Value.Count == 1);

                foreach (var single in singles)
                {
                    allergens = allergens.ToDictionary(
                        a => a.Key,
                        a => a.Key == single.Key ? a.Value : a.Value.Except(new[] { single.Value.Single() }).ToHashSet()
                    );
                }
            }

            return allergens;
        }
    }
}

