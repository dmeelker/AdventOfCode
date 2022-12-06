using Solution;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Theory]
        [InlineData("1+2", 3)]
        [InlineData("1+2*3", 9)]
        [InlineData("2+2+(5*2)", 14)]
        [InlineData("2+2+(5*(2*5))", 54)]
        public void InterpretPart1Expression(string expression, long expectedResult)
        {
            var result = Program.Part1(new[] { expression});
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1+2", 3)]
        [InlineData("3*1+2", 9)]
        [InlineData("(3*1)+2", 5)]
        public void InterpretPart2Expression(string expression, long expectedResult)
        {
            var result = Program.Part2(new[] { expression });
            Assert.Equal(expectedResult, result);
        }
    }
}
