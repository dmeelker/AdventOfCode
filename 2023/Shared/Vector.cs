using System.Diagnostics;

namespace AoC;

[DebuggerDisplay("{X},{Y}")]
public class Vector
{
    public double X { get; }
    public double Y { get; }

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Vector Add(Vector other) => new(X + other.X, Y + other.Y);
    public Vector Subtract(Vector other) => new(X - other.X, Y - other.Y);
    public Vector Multiply(double scalar) => new(X * scalar, Y * scalar);
    public double Length => Math.Sqrt((X * X) + (Y * Y));

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Vector other) return X == other.X && Y == other.Y;

        return false;
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
}
