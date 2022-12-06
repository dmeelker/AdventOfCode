using Solution;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class Tests
    {

        [Fact]
        public void Tile_Rotate()
        {
            var tile = new Grid
            (
                new[] {
                    new[] { true, true } ,
                    new[] { false, false }
                }
            );

            tile = tile.RotateRight();
            Assert.Equal(new[,] {
                     { false, true } ,
                     { false, true }
                }, tile.Image);

            tile = tile.RotateRight();
            Assert.Equal(new[,] {
                    { false, false} ,
                    { true, true }
                }, tile.Image);

            tile = tile.RotateRight();
            Assert.Equal(new[,] {
                    { true, false} ,
                    { true, false }
                }, tile.Image);

            tile = tile.RotateRight();
            Assert.Equal(new[,] {
                    { true, true} ,
                    { false, false }
                }, tile.Image);
        }

        [Fact]
        public void Tile_FlipHorizontal()
        {
            var tile = new Grid( new[] {
                    new[] { true, false } ,
                    new[] { true, false }
                }
            );

            tile = tile.FlipHorizontal();
            Assert.Equal(new[,] {
                    { false, true } ,
                    { false, true }
                }, tile.Image);
        }

        [Fact]
        public void Tile_FlipVertical()
        {
            var tile = new Grid
            (
                new[] {
                    new[] { true, true } ,
                    new[] { false, false  }
                }
            );

            tile = tile.FlipVertical();
            Assert.Equal(new[,] {
                    { false, false } ,
                    { true, true }
                }, tile.Image);
        }

        [Fact]
        public void IsMonster()
        {
            var grid = new Grid
            (
                Parser.ParseGrid(new[] { 
                    "                  # ",
                    "#    ##    ##    ###",
                    " #  #  #  #  #  #   "})
            );

            Assert.True(Program.IsMonster(grid, 0, 0));
        }

        [Fact]
        public void Part1()
        {
            var input = Parser.ParseInput(File.ReadAllText("input-small.txt"));
            var part1 = Program.Part1(input);

            Assert.Equal(20899048083289L, part1);
        }

        [Fact]
        public void Part2()
        {
            var input = Parser.ParseInput(File.ReadAllText("input-small.txt"));
            var part1 = Program.Part2(input);

            Assert.Equal(273, part1);
        }
    }
}
