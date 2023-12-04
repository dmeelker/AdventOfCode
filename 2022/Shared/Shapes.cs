namespace Shared
{
    public static class Shapes
    {
        public static IEnumerable<Point> Line(Point start, Point end)
        {
            var direction = end.Subtract(start).Sign();
            var location = start;

            yield return location;

            do
            {
                location = location.Add(direction);
                yield return location;
            } while (location != end);
        }

        public static IEnumerable<Point> Rect(Point start, Point size)
        {
            for (var y = 0; y < size.Y; y++)
            {
                for (var x = 0; x < size.X; x++)
                {
                    yield return new(start.X + x, start.Y + y);
                }
            }
        }

        public static IEnumerable<Point> Rect(Rect rect)
        {
            for (var y = rect.Y; y < rect.Y + rect.Height; y++)
            {
                for (var x = rect.X; x < rect.X + rect.Width; x++)
                {
                    yield return new(x, y);
                }
            }
        }


        public static IEnumerable<Point> Neighbours(Point location, bool includeDiagonals = true)
        {
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                        continue; // Skip self

                    if (!includeDiagonals && Math.Abs(x) == Math.Abs(y))
                        continue;

                    yield return new Point(location.X + x, location.Y + y);
                }
            }
        }
    }
}
