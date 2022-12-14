using System;
using System.Security.Cryptography;
using System.Text;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = "bgvyzdsv";
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(string input)
        {
            var md5 = MD5.Create();
            var index = 1;

            while (true)
            {
                var option = input + index.ToString();
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(option));
                var hashString = Convert.ToHexString(hashBytes);

                if (hashString.StartsWith("00000"))
                {
                    return index;
                }

                index++;
            }
        }

        public static int Part2(string input)
        {
            var md5 = MD5.Create();
            var index = 1;

            while (true)
            {
                var option = input + index.ToString();
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(option));
                var hashString = Convert.ToHexString(hashBytes);

                if (hashString.StartsWith("000000"))
                {
                    return index;
                }

                index++;
            }
        }
    }
}
