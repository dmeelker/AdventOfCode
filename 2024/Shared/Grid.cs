using System.Diagnostics;
using System.Text;

namespace Shared
{
    [DebuggerDisplay("{X},{Y}")]
    public record Point(int X, int Y)
    {
        public static readonly Point Up = new Point(0, -1);
        public static readonly Point UpLeft = new Point(-1, -1);
        public static readonly Point UpRight = new Point(1, -1);
        public static readonly Point Down = new Point(0, 1);
        public static readonly Point DownLeft = new Point(-1, 1);
        public static readonly Point DownRight = new Point(1, 1);
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Right = new Point(1, 0);

        public static Point operator +(Point a, Point b) => a.Add(b);

        public Point Add(Point other) => new Point(X + other.X, Y + other.Y);
        public Point Subtract(Point other) => new(X - other.X, Y - other.Y);
        public Point Sign() => new(Math.Sign(X), Math.Sign(Y));
        public int ManhattanDistanceTo(Point other) => Subtract(other).ManhattanDistance;

        public Point Multiply(int v)
        {
            return new Point(X * v, Y * v);
        }

        public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }

    public class Grid<T> where T : IEquatable<T>
    {
        [DebuggerDisplay("{Location} = {Value}")]
        public class CellReference<T2> where T2 : IEquatable<T2>
        {
            public Grid<T2> Grid { get; private set; }
            public Point Location { get; private set; }

            public CellReference(Grid<T2> grid, Point location)
            {
                Grid = grid;
                Location = location;
            }

            public T2 Value
            {
                get => Grid.Get(Location);
                set => Grid.Set(Location, value);
            }

            public override string ToString() => $"{Location}: {Value}";
        }

        private T[,] _data;
        public T DefaultValue { get; set; } = default!;

        public int Width => _data.GetLength(1);
        public int Height => _data.GetLength(0);

        public Grid(T[,] data, T defaultValue = default!)
        {
            _data = data;
            DefaultValue = defaultValue;
        }

        public Grid(int width, int height, T defaultValue = default!)
        {
            _data = new T[height, width];
            DefaultValue = defaultValue;
            Clear(defaultValue);
        }

        public Grid<T> Clone()
        {
            var clone = new Grid<T>(Width, Height, default!);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    clone.Set(x, y, Get(x, y));
                }
            }

            return clone;
        }

        public Grid<T2> Map<T2>(Func<CellReference<T>, T2> mapping) where T2 : IEquatable<T2>
        {
            var clone = new Grid<T2>(Width, Height, default!);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    clone.Set(x, y, mapping(new(this, new(x, y))));
                }
            }

            return clone;
        }

        public void Clear(T value)
        {
            ClearArray(_data, value);
        }

        private void ClearArray(T[,] arr, T value)
        {
            for (var x = 0; x < arr.GetLength(1); x++)
            {
                for (var y = 0; y < arr.GetLength(0); y++)
                {
                    arr[y, x] = value;
                }
            }
        }

        public bool Contains(int x, int y) => Contains(new(x, y));
        public bool Contains(Point point) => point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;

        public T Get(int x, int y) => Get(new(x, y));

        public T Get(Point location)
        {
            VerifyPointInBounds(location);
            return _data[location.Y, location.X];
        }

        public void Set(int x, int y, T value) => Set(new(x, y), value);

        public void Set(Point location, T value)
        {
            VerifyPointInBounds(location);
            _data[location.Y, location.X] = value;
        }

        private void VerifyPointInBounds(Point point)
        {
            if (!Contains(point))
                throw new ArgumentException($"Out of bounds: {point.X}, {point.Y}");
        }

        public IEnumerable<CellReference<T>> AllCells()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    yield return new(this, new(x, y));
                }
            }
        }

        public IEnumerable<CellReference<T>> Path(Point start, Point step, int? maxSteps = null)
        {
            Point location = start;
            int steps = 0;

            while (Contains(location))
            {
                yield return new(this, location);
                location = new(location.X + step.X, location.Y + step.Y);

                steps++;
                if (maxSteps.HasValue && steps >= maxSteps.Value)
                    break;
            }
        }

        public IEnumerable<IEnumerable<CellReference<T>>> Rows()
        {
            for (var y = 0; y < Height; y++)
            {
                yield return Row(y);
            }
        }

        public IEnumerable<CellReference<T>> Row(int y)
        {
            for (var x = 0; x < Width; ++x)
            {
                yield return new(this, new Point(x, y));
            }
        }

        public IEnumerable<IEnumerable<T>> RowValues()
        {
            for (var y = 0; y < Height; y++)
            {
                yield return RowValues(y);
            }
        }

        public IEnumerable<T> RowValues(int y)
        {
            for (var x = 0; x < Width; ++x)
            {
                yield return Get(x, y);
            }
        }

        public IEnumerable<IEnumerable<CellReference<T>>> Columns()
        {
            for (var x = 0; x < Width; ++x)
            {
                yield return Column(x);
            }
        }

        public IEnumerable<CellReference<T>> Column(int x)
        {
            for (var y = 0; y < Height; ++y)
            {
                yield return new(this, new Point(x, y));
            }
        }

        public IEnumerable<IEnumerable<T>> ColumnValues()
        {
            for (var x = 0; x < Width; ++x)
            {
                yield return ColumnValues(x);
            }
        }

        public IEnumerable<T> ColumnValues(int x)
        {
            for (var y = 0; y < Height; ++y)
            {
                yield return Get(x, y);
            }
        }

        public IEnumerable<CellReference<T>> Neighbours(Point location, bool includeDiagonals = true)
        {
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                        continue; // Skip self

                    if (!includeDiagonals && Math.Abs(x) == Math.Abs(y))
                        continue;

                    var local = new Point(location.X + x, location.Y + y);
                    if (Contains(local))
                    {
                        yield return new(this, local);
                    }
                }
            }
        }

        public IEnumerable<CellReference<T>> Edges()
        {
            return AllCells()
                .Where(cell => cell.Location.X == 0 || cell.Location.Y == 0 || cell.Location.X == Width - 1 || cell.Location.Y == Height - 1);
        }

        public IEnumerable<CellReference<T>> RectangleOutline(Rect rect)
        {
            for (var x = rect.X; x < rect.X + rect.Width; ++x)
            {
                for (var y = rect.Y; y < rect.Y + rect.Height; ++y)
                {
                    if (!(x == rect.X || x == rect.X + rect.Width - 1 || y == rect.Y || y == rect.Y + rect.Height - 1))
                    {
                        continue;
                    }

                    var location = new Point(x, y);
                    if (Contains(location))
                    {
                        yield return new(this, location);
                    }
                }
            }
        }

        public IEnumerable<CellReference<T>> Rectangle(Rect rect)
        {
            for (var x = rect.X; x < rect.X + rect.Width; ++x)
            {
                for (var y = rect.Y; y < rect.Y + rect.Height; ++y)
                {
                    var location = new Point(x, y);
                    if (Contains(location))
                    {
                        yield return new(this, location);
                    }
                }
            }
        }

        public IEnumerable<CellReference<T>> Flood(Point location)
        {
            var floodValue = Get(location);
            var closed = new HashSet<Point>();
            var open = new Queue<Point>();
            open.Enqueue(location);

            while (open.Count > 0)
            {
                var currentLocation = open.Dequeue();
                closed.Add(currentLocation);

                if (Get(currentLocation).Equals(floodValue))
                {
                    yield return new(this, currentLocation);
                    Consider(new(currentLocation.X - 1, currentLocation.Y));
                    Consider(new(currentLocation.X + 1, currentLocation.Y));
                    Consider(new(currentLocation.X, currentLocation.Y - 1));
                    Consider(new(currentLocation.X, currentLocation.Y + 1));
                }
            }

            void Consider(Point p)
            {
                if (!Contains(p))
                    return;

                if (closed.Contains(p))
                    return;

                if (!open.Contains(p))
                    open.Enqueue(p);
            }
        }

        public string Visualize()
        {
            var result = new StringBuilder();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    result.Append(Get(x, y));

                    if (x < Width - 1)
                    {
                        //result.Append(",");
                    }
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        public Grid<T> Grow(int size)
        {
            var newData = new Grid<T>(Width + (size * 2), Height + (size * 2), DefaultValue);

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    newData.Set(x + size, y + size, Get(x, y));
                }
            }

            return newData;
        }

        public static Grid<char> ParseCharGrid(IEnumerable<string> lines)
        {
            var chars = lines.Select(line => line.ToCharArray()).ToArray();

            var grid = new Grid<char>(chars[0].Length, chars.Length);

            for (var x = 0; x < grid.Width; x++)
            {
                for (var y = 0; y < grid.Height; y++)
                {
                    grid.Set(x, y, chars[y][x]);
                }
            }

            return grid;
        }

        public static Grid<int> ParseIntGrid(IEnumerable<string> lines)
        {
            var chars = lines.Select(line => line.ToCharArray()).ToArray();

            var grid = new Grid<int>(chars[0].Length, chars.Length);

            for (var x = 0; x < grid.Width; x++)
            {
                for (var y = 0; y < grid.Height; y++)
                {
                    grid.Set(x, y, int.Parse(chars[y][x].ToString()));
                }
            }

            return grid;
        }
    }
}
