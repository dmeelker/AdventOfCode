using Solution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void ParseInput()
        {
            var result = Parser.ParseLine("eseswwnwne"); // e, se, sw, w, nw, and ne

            Assert.Equal(new Point[] {
                Directions.East,
                Directions.SouthEast,
                Directions.SouthWest,
                Directions.West,
                Directions.NorthWest,
                Directions.NorthEast,
            }, result);
        }

        [Fact]
        public void Part1()
        {
            var result = Program.Part1(Parser.ParseInput(File.ReadAllText("input.txt")));

            Assert.Equal(287, result.Count());
        }

        [Fact]
        public void Part2()
        {
            var result = Program.Part1(Parser.ParseInput(File.ReadAllText("input.txt")));
            result = Program.Part2(result);
            Assert.Equal(3636, result.Count());
        }
    }
}
