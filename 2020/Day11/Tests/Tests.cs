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
            var result = Program.ParseInput("ABC\nDEF\n\n");

            Assert.Equal(new char[,] {
                    { 'A', 'B', 'C'},
                    { 'D', 'E', 'F'}
            }, result);
        }

        //[Fact]
        //public void CountAdjacentOccupiedSeats_OutOfBounds()
        //{
        //    var result = Simulator.CountAdjacentOccupiedSeats(new[,]{
        //            { '#'},
        //        }, 0, 0);

        //    Assert.Equal(0, result);
        //}

        //[Fact]
        //public void CountAdjacentOccupiedSeats_AllFloor()
        //{
        //    var result = Simulator1.CountAdjacentOccupiedSeats(new[,]{
        //            { '.', '.', '.'},
        //            { '.', '#', '.'},
        //            { '.', '.', '.'}
        //        }, 1, 1);

        //    Assert.Equal(0, result);
        //}

        //[Fact]
        //public void CountAdjacentOccupiedSeats_AllEmpty()
        //{
        //    var result = Simulator1.CountAdjacentOccupiedSeats(new[,]{
        //            { 'L', 'L', 'L'},
        //            { 'L', '#', 'L'},
        //            { 'L', 'L', 'L'}
        //        }, 1, 1);

        //    Assert.Equal(0, result);
        //}

        //[Fact]
        //public void CountAdjacentOccupiedSeats_AllOccupied()
        //{
        //    var result = Simulator1.CountAdjacentOccupiedSeats(new[,]{
        //            { '#', '#', '#'},
        //            { '#', '#', '#'},
        //            { '#', '#', '#'}
        //        }, 1, 1);

        //    Assert.Equal(8, result);
        //}

        [Fact]
        public void Simulate_EmptySeat()
        {
            var input = Program.ParseInput(@"LLL
LLL
LLL");

            var result = Simulator.SimulatePart1(input);

            Assert.Equal(Program.ParseInput(@"###
###
###"), result);
        }

        [Fact]
        public void Simulate1()
        {
            var input = Program.ParseInput(@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL");

            var result = Simulator.SimulatePart1(input);
            var r1 = result.AsString();
            Assert.Equal(Program.ParseInput(
@"#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##"), result);

            result = Simulator.SimulatePart1(result);
            var r2 = result.AsString();
            Assert.Equal(Program.ParseInput(
@"#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##"), result);

            result = Simulator.SimulatePart1(result);

            Assert.Equal(Program.ParseInput(@"#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##"), result);
        }


        [Fact]
        public void Simulate2_AllClear()
        {
            var input = Program.ParseInput(@"L");

            var result = Simulator.SimulatePart2(input);

            Assert.Equal(Program.ParseInput(@"#"), result);
        }

        [Fact]
        public void Simulate2_AllClear2()
        {
            var input = Program.ParseInput(@"...
.L.
...");

            var result = Simulator.SimulatePart2(input);

            Assert.Equal(Program.ParseInput(@"...
.#.
..."), result);
        }

        [Fact]
        public void Simulate2_AllAngles()
        {
            for(var x=0; x<=2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    if (x == 1 && y == 1)
                        continue;

                    var input = Program.ParseInput(@"...
.L.
...");

                    input[x, y] = '#';

                    var result = Simulator.SimulatePart2(input);

                    var expected = Program.ParseInput(@"...
.L.
...");
                    expected[x, y] = '#';
                    Assert.Equal(expected, result) ;
                }
            }
            


        }

        [Fact]
        public void Simulate2()
        {
            var input = Program.ParseInput(@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL");

            var result = Simulator.SimulatePart2(input);

            Assert.Equal(Program.ParseInput(@"#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##"), result);

            result = Simulator.SimulatePart2(result);
            var r2 = result.AsString();
            Assert.Equal(Program.ParseInput(
@"#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#"), result);

            result = Simulator.SimulatePart2(result);

            var r3 = result.AsString();
            Assert.Equal(Program.ParseInput(@"#.L#.##.L#
#L#####.LL
L.#.#..#..
##L#.##.##
#.##.#L.##
#.#####.#L
..#.#.....
LLL####LL#
#.L#####.L
#.L####.L#"), result);
        }
    }
}
