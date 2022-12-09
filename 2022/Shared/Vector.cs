using System.Numerics;

namespace AoC;

public class Vector<T> where T : INumber<T>
{
    public T X { get; }
    public T Y { get; }

    public Vector(T x, T y)
    {
        X = x;
        Y = y;
    }

    public Vector<T> Add(Vector<T> other) => new(X + other.X, Y + other.Y);
    public Vector<T> Multiply(T scalar) => new(X * scalar, Y * scalar);

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Vector<T> other) return X == other.X && Y == other.Y;

        return false;
    }
}
