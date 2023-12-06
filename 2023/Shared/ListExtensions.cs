namespace Shared
{
    public static class ListExtensions
    {
        public static T RemoveLast<T>(this List<T> list)
        {
            var last = list[^1];
            list.RemoveAt(list.Count - 1);
            return last;
        }
    }
}
