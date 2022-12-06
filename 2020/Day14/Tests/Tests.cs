using Solution;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void ParseInput()
        {
            var result = Parser.ParseInput("");
        }

        [Fact]
        public void Part1()
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var result = Program.Part1(input);

            Assert.Equal(11327140210986L, result);
        }

        [Fact]
        public void Part2()
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var result = Program.Part2(input);

            Assert.Equal(2308180581795L, result);
        }
    }
}
