namespace Shared.Extensions
{
    public static class ListExt
    {
        public static bool MoveUp<T>(this IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (index <= 0 || index >= list.Count)
                return false;

            (list[index - 1], list[index]) = (list[index], list[index - 1]);
            return true;
        }

        public static bool MoveDown<T>(this IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (index < 0 || index >= list.Count - 1)
                return false;

            (list[index], list[index + 1]) = (list[index + 1], list[index]);
            return true;
        }

        public static bool Move<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (oldIndex < 0 || oldIndex >= list.Count)
                return false;

            if (newIndex < 0 || newIndex >= list.Count)
                return false;

            if (oldIndex == newIndex)
                return true;

            T item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);

            return true;
        }
    }
}
