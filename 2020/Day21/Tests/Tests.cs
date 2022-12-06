using Solution;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Solve()
        {
            var result = Program.Solve(Parser.ParseInput(File.ReadAllText("input.txt")));
            Assert.Equal(2211, result.Item1);
            Assert.Equal("vv,nlxsmb,rnbhjk,bvnkk,ttxvphb,qmkz,trmzkcfg,jpvz", result.Item2);
        }
    }
}
