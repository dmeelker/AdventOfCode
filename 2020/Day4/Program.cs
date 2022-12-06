using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    public class FieldDefinition
    {
        public string Name { get; set; }
        public Func<string, bool> Check { get; set; }

        public FieldDefinition(string name, Func<string, bool> check)
        {
            Name = name;
            Check = check;
        }
    }

    public class Program
    {
        public static FieldDefinition[] _fields = new[] { 
            new FieldDefinition("byr", input => ValidateRange(input, 1920, 2002)),
            new FieldDefinition("iyr", input => ValidateRange(input, 2010, 2020)),
            new FieldDefinition("eyr", input => ValidateRange(input, 2020, 2030)),
            new FieldDefinition("hgt", ValidateHeight),
            new FieldDefinition("hcl", input => Regex.IsMatch(input, "^#[0-9a-f]{6}$")),
            new FieldDefinition("ecl", input => Regex.IsMatch(input, "^amb|blu|brn|gry|grn|hzl|oth$")),
            new FieldDefinition("pid", input => Regex.IsMatch(input, "^[0-9]{9}$")),
        };

        static void Main(string[] args)
        {
            var part1ValidCount = Load(File.ReadAllText("input.txt"))
                .Where(HasMandatoryFields)
                .Count();

            var part2ValidCount = Load(File.ReadAllText("input.txt"))
                .Where(ValidateFields)
                .Count();

            Console.WriteLine($"Part 1 result: {part1ValidCount}");
            Console.WriteLine($"Part 2 result: {part2ValidCount}");
        }

        public static bool ValidateFields(Dictionary<string, string> data)
        {
            return _fields.All(field => data.ContainsKey(field.Name) && field.Check(data[field.Name]));
        }

        static bool HasMandatoryFields(Dictionary<string, string> data)
        {
            return _fields.All(field => data.ContainsKey(field.Name));
        }

        public static IEnumerable<Dictionary<string,  string>> Load(string text)
        {
            return text.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(section =>
                    section.Split(new string[] { " ", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(entry => entry.Split(':'))
                        .ToDictionary(entry => entry[0], entry => entry[1])
                );
        }

        static bool ValidateRange(string value, int min, int max)
        {
            return int.TryParse(value, out var number) && number >= min && number <= max;
        }

        static bool ValidateHeight(string input)
        {
            var match = Regex.Match(input, @"(?<value>[\d]{2,3})(?<unit>cm|in)");
            if (!match.Success)
                return false;

            var value = match.Groups["value"].Value;

            return match.Groups["unit"].Value switch {
                "cm" => ValidateRange(value, 150, 193),
                "in" => ValidateRange(value, 59, 76),
                _ => false,
            };
        }
    }
}
