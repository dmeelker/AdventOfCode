namespace AoC;

class Vector
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector Add(Vector other)
    {
        return new(X + other.X, Y + other.Y);
    }
    public override int GetHashCode()
    {
        return X * Y;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Vector other) return X == other.X && Y == other.Y;

        return false;
    }
}
