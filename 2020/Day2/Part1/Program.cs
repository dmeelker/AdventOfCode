using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Password
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public char Character { get; set; }
        public string Value { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var passwords = LoadPasswords("input.txt");
            var validPasswords = passwords.Where(ValidatePassword).Count();
            var invalidPasswords = passwords.Length - validPasswords;

            Console.WriteLine($"Valid: {validPasswords}, invalid: {invalidPasswords}");
        }

        private static bool ValidatePassword(Password password)
        {
            var count = CountCharacters(password.Value, password.Character);

            return count >= password.Min && count <= password.Max;
        }

        private static int CountCharacters(string input, char character)
        {
            return input.ToCharArray().Where(chr => chr == character).Count();
        }

        private static Password[] LoadPasswords(string file)
        {
            return File.ReadAllLines(file)
                .Select(ParsePassword)
                .ToArray();
        }

        private static Password ParsePassword(string line)
        {
            var match = Regex.Match(line, @"(?<min>\d+)\-(?<max>\d+) (?<char>.): (?<password>.*)");

            return new Password { 
                Min = Convert.ToInt32(match.Groups["min"].Value),
                Max = Convert.ToInt32(match.Groups["max"].Value),
                Character = match.Groups["char"].Value[0],
                Value = match.Groups["password"].Value
            };
        }
    }
}
