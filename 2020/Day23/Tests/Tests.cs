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
            var result = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal("97342568", Program.Part1(result));
        }
    }
}
