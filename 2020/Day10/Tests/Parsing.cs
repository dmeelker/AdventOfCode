using Solution;
using System;
using Xunit;

namespace Tests
{
    public class Parsing
    {
        [Fact]
        public void ParseInput()
        {
            var result = Program.ParseInput(@"10
20
30

40");

            Assert.Equal(new long[] { 10, 20, 30, 40}, result);
        }

        [Fact]
        public void Part1_1()
        {
            var result = Program.Part1(Program.PreprocessInput(new long[] {16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 }));

            Assert.Equal(7 * 5, result);
        }

        [Fact]
        public void Part1_2()
        {
            var result = Program.Part1(Program.PreprocessInput(new long[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3}));

            Assert.Equal(22 * 10, result);
        }

        [Fact]
        public void Part2_1()
        {
            var result = Program.Part2(Program.PreprocessInput(new long[] { 1, 4, 5, 6, 7, 10, 11, 12, 15, 16, 19 }));

            Assert.Equal(8, result);
        }

        [Fact]
        public void Part2_2()
        {
            var result = Program.Part2(Program.PreprocessInput(new long[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3 }));

            Assert.Equal(19208, result);
        }
    }
}
