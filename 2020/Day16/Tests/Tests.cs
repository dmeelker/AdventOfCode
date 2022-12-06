using Solution;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Part1()
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal(22000L, Program.Part1(input));
        }

        [Fact]
        public void Part2()
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal(410460648673L, Program.Part2(input));
        }
    }
}
