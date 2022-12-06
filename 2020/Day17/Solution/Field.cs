using System;
using System.Collections.Generic;
using System.Text;

namespace Solution
{
    public class Field
    {
        private readonly bool[,,,] _data;
        private readonly int _x;
        private readonly int _z;
        private readonly int _y;
        private readonly int _w;

        public Field(int x, int z, int y, int w)
        {
            _x = x;
            _z = z;
            _y = y;
            _w = w;
            _data = new bool[x, z, y, w];
        }

        public void Set(Point point, bool value)
        {
            _data[point.X, point.Z, point.Y, point.W] = value;
        }

        public bool Get(Point point)
        {
            return _data[point.X, point.Z, point.Y, point.W];
        }

        public void VisitCells(Action<Point, bool> visitor)
        {
            for(var x=0; x < _x; x++)
            {
                for (var z = 0; z < _z; z++)
                {
                    for (var y = 0; y < _y; y++)
                    {
                        for (var w = 0; w < _w; w++)
                        {
                            visitor(new Point(x, y, z, w), _data[x, z, y, w]);
                        }
                    }
                }
            }
        }

        public IEnumerable<Point> GetAdjacentCells(Point point)
        {
            for (var y = -1; y <= 1; y++)
            {
                for (var z = -1; z <= 1; z++)
                {
                    for (var x = -1; x <= 1; x++)
                    {
                        for (var w = -1; w <= 1; w++)
                        {
                            var adjacentCell = point.Add(x, y, z, w);
                            if (adjacentCell.Equals(point))
                                continue;

                            if (ContainsPoint(adjacentCell))
                                yield return adjacentCell;
                        }
                    }
                }
            }
        }

        public int CountActiveCells()
        {
            var total = 0;

            VisitCells((_, active) => total += active ? 1 : 0);
            return total;
        }

        public bool ContainsPoint(Point point)
        {
            return point.X >= 0 && point.X < _x &&
                point.Z >= 0 && point.Z < _z &&
                point.Y >= 0 && point.Y < _y &&
                point.W >= 0 && point.W < _w;
        }

        public Field Clone()
        {
            var clone = new Field(_x, _z, _y, _w);

            VisitCells((point, value) => clone.Set(point, value));

            return clone;
        }

        public Field Expand3d()
        {
            var clone = new Field(_x+2, _z+2, _y+2, 1);

            VisitCells((point, value) => clone.Set(point.Add(1, 1, 1, 0), value));

            return clone;
        }

        public Field Expand4d()
        {
            var clone = new Field(_x + 2, _z + 2, _y + 2, _w + 2);

            VisitCells((point, value) => clone.Set(point.Add(1, 1, 1, 1), value));

            return clone;
        }
    }

    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public int W { get; private set; }

        public Point(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Point Add(int x, int y, int z, int w)
        {
            return new Point(X + x, Y + y, Z + z, W +w);
        }

        public bool Equals(Point other)
        {
            return X == other.X &&
                Y == other.Y &&
                Z == other.Z &&
                W == other.W;
        }
    }
}

