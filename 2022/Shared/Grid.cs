using System.Diagnostics;
using System.Text;

namespace Shared
{
    [DebuggerDisplay("{X},{Y}")]
    public record Point(int X, int Y);

    public class Grid<T>
    {
        [DebuggerDisplay("{Location} = {Value}")]
        public class CellReference<T2>
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
        private T _defaultValue;

        public int Width => _data.GetLength(0);
        public int Height => _data.GetLength(1);

        public Grid(int width, int height, T defaultValue)
        {
            _data = new T[width, height];
            _defaultValue = defaultValue;
            Clear(defaultValue);
        }

        public Grid<T> Clone()
        {
            var clone = new Grid<T>(Width, Height, default!);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    clone._data[x, y] = _data[x, y];
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
            for (var x = 0; x < arr.GetLength(0); x++)
            {
                for (var y = 0; y < arr.GetLength(1); y++)
                {
                    arr[x, y] = value;
                }
            }
        }

        public bool Contains(int x, int y) => Contains(new(x, y));
        public bool Contains(Point point) => point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;

        public T Get(int x, int y) => Get(new(x, y));

        public T Get(Point location)
        {
            VerifyPointInBounds(location);
            return _data[location.X, location.Y];
        }

        public void Set(int x, int y, T value) => Set(new(x, y), value);

        public void Set(Point location, T value)
        {
            VerifyPointInBounds(location);
            _data[location.X, location.Y] = value;
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

        public IEnumerable<CellReference<T>> Path(Point start, Point step)
        {
            Point location = start;

            while (Contains(location))
            {
                yield return new(this, location);
                location = new(location.X + step.X, location.Y + step.Y);
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

        public string Visualize()
        {
            var result = new StringBuilder();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    result.Append(_data[x, y]);

                    if (x < Width - 1)
                    {
                        result.Append(",");
                    }
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        public void Grow(int size)
        {
            var newData = new T[Width + (size * 2), Height + (size * 2)];
            ClearArray(newData, _defaultValue);

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    newData[x + size, y + size] = _data[x, y];
                }
            }

            _data = newData;
        }
    }
}
