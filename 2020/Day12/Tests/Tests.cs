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
            var result = Program.ParseInput("");
        }

        [Fact]
        public void RotateVector()
        {
            var v = new Vector(10, 0);
            v = v.Rotate(90);

            Assert.Equal(0, v.X);
            Assert.Equal(10, v.Y);
        }
    }
}
