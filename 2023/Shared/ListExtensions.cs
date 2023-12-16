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

        public static bool Replace<T>(this List<T> list, T oldItem, T newItem)
        {
            var index = list.IndexOf(oldItem);
            if (index == -1)
            {
                return false;
            }

            list.RemoveAt(index);
            list.Insert(index, newItem);
            return true;
        }

        public static void ReplaceOrAdd<T>(this List<T> list, T oldItem, T newItem)
        {
            if (!list.Replace(oldItem, newItem))
            {
                list.Add(newItem);
            }
        }
    }

    public static class StackExtensions
    {
        public static void Push<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                stack.Push(item);
            }
        }
    }
}
