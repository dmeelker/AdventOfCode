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
    }
}
