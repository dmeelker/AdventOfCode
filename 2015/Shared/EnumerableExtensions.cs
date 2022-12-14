namespace Shared
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Splits an enumerable into two equal parts
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> input)
        {
            if (input.Count() % 2 != 0)
                throw new ArgumentException("Input has to contain an even number of elements");

            var halfSize = input.Count() / 2;

            return new[] {
                input.Take(halfSize),
                input.Skip(halfSize)
            };
        }

        public static IEnumerable<T[]> SlidingWindow<T>(this IEnumerable<T> input, int windowSize)
        {
            var values = input.ToArray();

            if (values.Length < windowSize)
            {
                throw new ArgumentException("Size is less than window size");
            }

            for (var i = 0; i < values.Length - windowSize + 1; i++)
            {
                yield return values[i..(i + windowSize)];
            }
        }
    }
}
