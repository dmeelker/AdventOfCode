using Solution;
using System;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void ParseInput()
        {
            var result = Parser.ParseInput("1,2,3");
            Assert.Equal(new long[] {1,2,3 }, result);
        }

        [Theory]
        [InlineData("0,3,6", 436)]
        [InlineData("1,3,2", 1)]
        [InlineData("3,2,1", 438)]
        [InlineData("3,1,2", 1836)]
        [InlineData("2,0,6,12,1,3", 1428)]
        public void Solve(string input, long expectedNumber)
        {
            var result = Program.Solve(Parser.ParseInput(input), 2020);

            Assert.Equal(expectedNumber, result);
        }
    }
}
