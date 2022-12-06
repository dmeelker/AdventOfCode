using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Password
    {
        public int Position1 { get; set; }
        public int Position2 { get; set; }
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
            var position1 = password.Value[password.Position1 - 1] == password.Character;
            var position2 = password.Value[password.Position2 - 1] == password.Character;

            return position1 ^ position2;
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
            var match = Regex.Match(line, @"(?<start>\d+)\-(?<end>\d+) (?<char>.): (?<password>.*)");

            return new Password { 
                Position1 = Convert.ToInt32(match.Groups["start"].Value),
                Position2 = Convert.ToInt32(match.Groups["end"].Value),
                Character = match.Groups["char"].Value[0],
                Value = match.Groups["password"].Value
            };
        }
    }
}
