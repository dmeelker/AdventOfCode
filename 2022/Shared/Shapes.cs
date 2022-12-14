namespace Shared
{
    public static class Shapes
    {
        public static IEnumerable<Point> OrthogonalLine(Point start, Point end)
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
    }
}
