using System;
using System.Diagnostics.CodeAnalysis;

namespace Solution
{
    public class Point : IEquatable<Point>
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point Add(Point p)
        {
            return new Point(X + p.X, Y + p.Y);
        }

        public override bool Equals(object obj)
        {
            if(obj is Point)
            {
                var p = (Point)obj;
                return X == p.X && Y == p.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Y << 16) ^ X;
        }

        public bool Equals([AllowNull] Point other)
        {
            return other is Point && Equals((Object)other);
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
