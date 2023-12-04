namespace Shared
{
    public record Rect(int X, int Y, int Width, int Height)
    {
        public bool IsAdjacentTo(Point point)
        {
            return Shapes.Rect(new Point(X - 1, Y - 1), new Point(Width + 2, Height + 2)).Any(p => p == point);
        }

        public Rect Grow(int size = 1)
        {
            return new Rect(X - size, Y - size, Width + (size * 2), Height + (size * 2));
        }
    };
}
