using System;
using System.Collections.Generic;

namespace InfraManager.Core.Extensions
{
    public static class ListExtensions
    {
        #region Remove
        public static bool Remove<T>(this List<T> list, Predicate<T> match)
        {
            if (list == null)
                return false;
            int index = list.FindIndex(match);
            if (index < 0) return false;
            list.RemoveAt(index);
            return true;
        }
        #endregion

        #region Sort
        public static void Sort<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = System.Collections.Generic.Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(x), selector(y)));
        }
        #endregion

        #region SortDescending
        static void SortDescending<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = System.Collections.Generic.Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(y), selector(x)));
        }
        #endregion
    }
}
