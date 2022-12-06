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
            var result = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal(35202, Program.Part1(result));
        }

        [Fact]
        public void Part2()
        {
            var result = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal(32317, Program.Part2(result));
        }
    }
}
