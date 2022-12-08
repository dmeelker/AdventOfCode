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
}
