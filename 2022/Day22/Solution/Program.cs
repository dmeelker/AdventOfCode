using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));

            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static int Part1(Input input)
        {
            var world = input.World.Clone();
            world.Grow(1);

            var location = world.GetStartLocation();
            var direction = Direction.Right;

            foreach (var instruction in input.Instructions)
            {
                switch (instruction.Type)
                {
                    case InstructionType.Move:
                        var v = direction.ToPoint();

                        for (var i = 0; i < instruction.Steps!.Value; i++)
                        {
                            var newLocation = location.Add(v);
                            if (world.Get(newLocation) == ' ')
                            {
                                newLocation = world.Wrap(newLocation, direction);
                            }

                            if (world.Get(newLocation) == '#')
                            {
                                break;
                            }

                            location = newLocation;
                        }
                        break;
                    case InstructionType.TurnLeft:
                        direction = direction.TurnLeft();
                        break;
                    case InstructionType.TurnRight:
                        direction = direction.TurnRight();
                        break;
                }
            }

            return (location.Y * 1000) + (location.X * 4) + (int)direction;
        }

        public static int Part2(Input input)
        {
            //var allMappings = new List<EdgeMapping>()
            //{
            //    new EdgeMapping(
            //        new Edge(new(0, 4), new(0, 4), Point.Left),
            //        new Edge(new(15, 11), new(-4, 0), Point.Down)
            //    ),
            //    new EdgeMapping(
            //        new Edge(new(15, 8), new(-4, 0), Point.Up),
            //        new Edge(new(11, 4), new(0, 4), Point.Right)
            //    )
            //};

            var allMappings = new List<EdgeMapping>()
            {
                //Light green
                new EdgeMapping(
                    new Edge(new(50, 50), new(0, 50), Point.Left),
                    new Edge(new(0, 100), new(50, 0), Point.Up)
                ),
                // Green
                new EdgeMapping(
                    new Edge(new(50, 150), new(50, 0), Point.Down),
                    new Edge(new(50, 150), new(0, 50), Point.Right)
                ),
                // Orange
                new EdgeMapping(
                    new Edge(new(100,50), new(50, 0), Point.Down),
                    new Edge(new(100,50), new(0, 50), Point.Right)
                ),
                // Blue
                new EdgeMapping(
                    new Edge(new(150, 50), new(0, -50), Point.Right),
                    new Edge(new(100,100), new(0, 50), Point.Right)
                ),
                // Light blue
                new EdgeMapping(
                    new Edge(new(0,100), new(0, 50), Point.Left),
                    new Edge(new(50,50), new(0, -50), Point.Left)
                ),
                // Purple
                new EdgeMapping(
                    new Edge(new(0,200), new(0, -50), Point.Left),
                    new Edge(new(100,0), new(-50,0), Point.Up)
                ),
                // Yellow
                new EdgeMapping(
                    new Edge(new(0,200), new(50, 0), Point.Down),
                    new Edge(new(100,0), new(50, 0), Point.Up)
                )
            };


            var world = input.World.Clone();
            var location = world.GetStartLocation();
            var direction = Direction.Right;

            //input.Instructions.Clear();
            //input.Instructions.Add(new Instruction(InstructionType.Move, 4));
            //input.Instructions.Add(new Instruction(InstructionType.TurnLeft, null));
            //input.Instructions.Add(new Instruction(InstructionType.Move, 1));
            //input.Instructions.Add(new Instruction(InstructionType.TurnRight, null));
            //input.Instructions.Add(new Instruction(InstructionType.Move, 4));

            foreach (var instruction in input.Instructions)
            {
                switch (instruction.Type)
                {
                    case InstructionType.Move:
                        var v = direction.ToPoint();

                        for (var i = 0; i < instruction.Steps!.Value; i++)
                        {
                            Point newLocation;
                            var newDirection = direction;
                            var newV = v;

                            var edge = allMappings.Select(m => m.FindMatchingEdge(location, v)).Where(m => m != null).FirstOrDefault();

                            if (edge != null)
                            {
                                var localPoint = edge.LocalPoint(location);
                                var worldPoint = edge.OpposingEdge.ToWorldPoint(localPoint);

                                newLocation = worldPoint;
                                newDirection = DirectionHelpers.FromPoint(edge.OpposingEdge.InVector);
                                newV = newDirection.ToPoint();
                            }
                            else
                            {
                                newLocation = location.Add(v);
                            }

                            if (world.Get(newLocation) == ' ')
                            {
                                throw new Exception();
                                newLocation = world.Wrap(newLocation, direction);
                            }

                            if (world.Get(newLocation) == '#')
                            {
                                break;
                            }

                            location = newLocation;
                            direction = newDirection;
                            v = newV;

                            //Console.Clear();
                            //Console.WriteLine(Visualize(world, location, direction));
                            //Console.ReadKey();
                        }

                        break;
                    case InstructionType.TurnLeft:
                        direction = direction.TurnLeft();
                        //Console.Clear();
                        //Console.WriteLine(Visualize(world, location, direction));
                        //Console.ReadKey();

                        break;
                    case InstructionType.TurnRight:
                        direction = direction.TurnRight();
                        //Console.Clear();
                        //Console.WriteLine(Visualize(world, location, direction));
                        //Console.ReadKey();

                        break;
                }


            }

            return (location.Y * 1000) + (location.X * 4) + (int)direction;
        }
        public static string Visualize(Grid<char> grid, Point location, Direction direction)
        {
            var result = new StringBuilder();

            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    if (location.Equals(new(x, y)))
                    {
                        result.Append(direction switch
                        {
                            Direction.Right => ">",
                            Direction.Down => "v",
                            Direction.Left => "<",
                            Direction.Up => "^"
                        });
                    }
                    else
                    {
                        result.Append(grid.Get(x, y));
                    }
                }
                result.AppendLine();
            }

            return result.ToString();
        }



        /*
           #
         ###
           ##
         */

        /*
          ##
          #
         ##
          #
        */

        private class Edge
        {
            public Point Start { get; set; }
            public Point End { get; }
            public Point Vector { get; set; }
            public Point OutVector { get; }
            public Point InVector { get; }
            public Edge OpposingEdge { get; set; }

            public Edge(Point start, Point vector, Point vOut)
            {
                Start = start;
                Vector = vector;
                End = Start.Add(Vector);

                //OutVector = Vector.ToUnit().Rotate(Math.PI / 2.0);
                //InVector = Vector.ToUnit().Rotate(-(Math.PI / 2.0));
                InVector = vOut.Multiply(-1);
                OutVector = vOut;
            }

            public bool Matches(Point point, Point v)
            {
                if (v != OutVector)
                    return false;

                var length = Vector.Length;
                var step = Vector.ToUnit();
                var location = Start;

                for (var i = 0; i < length; i++)
                {
                    if (location == point)
                        return true;

                    location = location.Add(step);
                }

                return false;
            }

            public Point LocalPoint(Point worldPoint)
            {
                if (worldPoint.X > Start.X || worldPoint.Y > Start.Y)
                    return worldPoint.Subtract(Start);
                else
                    return Start.Subtract(worldPoint);
            }

            public Point ToWorldPoint(Point localPoint)
            {
                var l = (int)localPoint.Length;
                var offset = Vector.ToUnit().Multiply(l);
                return Start.Add(offset);
            }
        }

        private class EdgeMapping
        {
            public Edge[] Edges { get; set; }

            public EdgeMapping(params Edge[] edges)
            {
                Edges = edges;
                Edges[0].OpposingEdge = Edges[1];
                Edges[1].OpposingEdge = Edges[0];
            }

            public Edge? FindMatchingEdge(Point point, Point v)
            {
                return Edges.FirstOrDefault(e => e.Matches(point, v));
            }
        }

        private static Cube ConstructCube(Grid<char> input)
        {


            var faces = PrepareFaceGrids(input);
            faces.Get(2, 1).Name = "A";
            faces.Get(1, 1).Name = "B";
            faces.Get(3, 2).Name = "C";

            faces.Get(2, 0).Name = "D";
            faces.Get(2, 2).Name = "E";
            faces.Get(0, 1).Name = "F";

            foreach (var cell in faces.AllCells().Where(c => c.Value != null))
            {
                var face = cell.Value!;

                foreach (var neighbourCell in faces.Neighbours(cell.Location, false))
                {
                    if (neighbourCell.Value == null)
                    {
                        continue;
                    }

                    var neighbourFace = neighbourCell.Value;
                    if (neighbourCell.Location.Y < cell.Location.Y)
                    {
                        face.EnsureConnected(neighbourFace, Direction.Up, Direction.Down);
                    }
                    if (neighbourCell.Location.Y > cell.Location.Y)
                    {
                        face.EnsureConnected(neighbourFace, Direction.Down, Direction.Up);
                    }
                    if (neighbourCell.Location.X < cell.Location.X)
                    {
                        face.EnsureConnected(neighbourFace, Direction.Left, Direction.Right);
                    }
                    if (neighbourCell.Location.X > cell.Location.X)
                    {
                        face.EnsureConnected(neighbourFace, Direction.Right, Direction.Left);
                    }

                }
            }

            while (!faces.AllCells().Where(c => c.Value != null).All(c => c.Value!.AllNeighboursConnected))
            {
                foreach (var cell in faces.AllCells().Where(c => c.Value != null))
                {
                    var face = cell.Value!;

                    FaceRelation(face.TopNeighbour, face.LeftNeighbour, Direction.Left, Direction.Up);
                    FaceRelation(face.TopNeighbour, face.RightNeighbour, Direction.Right, Direction.Up);
                    FaceRelation(face.BottomNeighbour, face.LeftNeighbour, Direction.Left, Direction.Down);
                    //FaceRelation(face.BottomNeighbour, face.RightNeighbour, Direction.Right, Direction.Down);

                    //FaceRelation(face.BottomNeighbour?.LeftNeighbour, face, Direction.Right, Direction.Left);
                    //FaceRelation(face.BottomNeighbour?.RightNeighbour, face, Direction.Left, Direction.Right);
                    //FaceRelation(face.TopNeighbour?.LeftNeighbour, face, Direction.Right, Direction.Left);
                    //FaceRelation(face.TopNeighbour?.RightNeighbour, face, Direction.Left, Direction.Right);
                }

            }

            return null!;
        }

        private static void FaceRelation(CubeFace? localFace, CubeFace? otherFace, Direction localDirection, Direction remoteDirection)
        {
            if (localFace != null && otherFace != null)
                localFace.EnsureConnected(otherFace, localDirection, remoteDirection);
        }
        private static Grid<CubeFace?> PrepareFaceGrids(Grid<char> input)
        {
            var faceSize = input.Rows().Min(row => string.Concat(row.Select(cell => cell.Value)).Trim().Length);
            var facesX = input.Width / faceSize;
            var facesY = input.Height / faceSize;

            var faceGrids = new Grid<CubeFace?>(facesX, facesY, null);

            for (var y = 0; y < facesY; y++)
            {
                for (var x = 0; x < facesX; x++)
                {
                    var faceX = x * faceSize;
                    var faceY = y * faceSize;

                    if (input.Contains(faceX, faceY) && input.Get(faceX, faceY) != ' ')
                    {
                        faceGrids.Set(x, y, new(input.Copy(new(faceX, faceY, faceSize, faceSize))));
                    }
                }
            }

            return faceGrids;
        }
    }

    public class Cube
    {
        public CubeFace[] Faces { get; }

        public Cube(CubeFace[] faces)
        {
            Faces = faces;
        }
    }

    [DebuggerDisplay("{Name}")]
    public class CubeFace
    {
        public string Name { get; set; }
        public Grid<char> Grid { get; }
        public CubeFace?[] Neighbours { get; } = new CubeFace?[4];
        public CubeFace TopNeighbour => Neighbours[(int)Direction.Up];
        public CubeFace BottomNeighbour => Neighbours[(int)Direction.Down];
        public CubeFace LeftNeighbour => Neighbours[(int)Direction.Left];
        public CubeFace RightNeighbour => Neighbours[(int)Direction.Right];

        public bool AllNeighboursConnected => Neighbours.All(n => n != null); // TopNeighbour != null && BottomNeighbour != null && LeftNeighbour != null && RightNeighbour != null;

        public CubeFace(Grid<char> grid)
        {
            Grid = grid;
        }

        public void EnsureConnected(CubeFace otherFace, Direction localDirection, Direction remoteDirection)
        {
            if (otherFace == this)
                throw new ArgumentException("Faces cannot connect to self");

            Neighbours[(int)localDirection] = otherFace;
            otherFace.Neighbours[(int)remoteDirection] = this;
        }
    }

    public static class DirectionHelpers
    {
        public static Direction TurnRight(this Direction direction)
        {
            return (Direction)(((int)direction + 1) % 4);
        }

        public static Direction TurnLeft(this Direction direction)
        {
            var newDirection = (int)direction - 1;
            if (newDirection < 0)
            {
                newDirection = 3;
            }
            return (Direction)newDirection;
        }

        public static Point ToPoint(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Point.Up,
                Direction.Right => Point.Right,
                Direction.Down => Point.Down,
                Direction.Left => Point.Left
            };
        }

        public static Direction FromPoint(Point point)
        {
            if (point.X < 0) return Direction.Left;
            if (point.X > 0) return Direction.Right;
            if (point.Y < 0) return Direction.Up;
            if (point.Y > 0) return Direction.Down;

            throw new Exception();
        }
    }

    public static class GridHelpers
    {
        public static Point GetStartLocation(this Grid<char> grid)
        {
            return grid.Line(1).First(c => c.Value == '.').Location;
        }

        public static Point Wrap(this Grid<char> grid, Point location, Direction direction)
        {
            return direction switch
            {
                Direction.Up => grid.Column(location.X).Reverse().First(c => c.Value != ' ').Location,
                Direction.Right => grid.Row(location.Y).First(c => c.Value != ' ').Location,
                Direction.Down => grid.Column(location.X).First(c => c.Value != ' ').Location,
                Direction.Left => grid.Row(location.Y).Reverse().First(c => c.Value != ' ').Location,
            };
        }
    }
}
