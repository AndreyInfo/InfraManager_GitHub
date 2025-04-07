using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(var item in enumeration)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Union<T>(
            this IEnumerable<T> enumeration,
            T singleElement)
        {
            return enumeration.Union(new[] { singleElement });
        }

        public static IEnumerable<T> UnionIf<T>(
            this IEnumerable<T> first,
            bool union,
            IEnumerable<T> second)
        {
            return union ? first.Union(second) : first;
        }

        public static IEnumerable<T> UnionIf<T>(
            this IEnumerable<T> first,
            bool union,
            T singleElement)
        {
            return first.UnionIf(union, new[] { singleElement });
        }

        public static IQueryable<T> Where<T>(
            this IQueryable<T> query,
            IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            var result = query;

            foreach(var predicate in predicates)
            {
                result = result.Where(predicate);
            }

            return result;
        }
    }
}
