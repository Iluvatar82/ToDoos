namespace Framework.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var enumerator = enumerable.GetEnumerator();
            var index = 0;
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                action(current, index++);
                yield return current;
                index++;
            }
        }
    }
}
