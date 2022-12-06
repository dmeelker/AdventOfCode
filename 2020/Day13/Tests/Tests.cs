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
            var result = Program.ParseInput(@"939
7,13,x,x,59,x,31,19");

            Assert.Equal(939, result.departureTime);
            Assert.Equal(new int?[] { 7, 13, null, null, 59, null, 31, 19 }, result.busIds);
        }

        [Fact]
        public void Part1()
        {
            var result = Program.Part1(939, new int?[] { 59, 31, 19, 13, 7 });

            Assert.Equal(295, result);
        }

        [Fact]
        public void Part2_1()
        {
            var result = Program.Part2(new int?[] { 7, 13, null, null, 59, null, 31, 19 });

            Assert.Equal((ulong)1068781, result);
        }

        [Fact]
        public void Part2_2()
        {
            var result = Program.Part2(new int?[] { 17, null, 13, 19 });

            Assert.Equal((ulong)3417, result);
        }

        [Fact]
        public void Part2_3()
        {
            var result = Program.Part2(new int?[] { 67, 7, 59, 61 });

            Assert.Equal((ulong)754018, result);
        }

        [Fact]
        public void Part2_4()
        {
            var result = Program.Part2(new int?[] { 67, null, 7, 59, 61 });

            Assert.Equal((ulong)779210, result);
        }

        [Fact]
        public void Part2_5()
        {
            var result = Program.Part2(new int?[] { 67, 7, null, 59, 61 });

            Assert.Equal((ulong)1261476, result);
        }

        [Fact]
        public void Part2_6()
        {
            var result = Program.Part2(new int?[] { 1789, 37, 47, 1889 });

            Assert.Equal((ulong)1202161486, result);
        }
    }
}
